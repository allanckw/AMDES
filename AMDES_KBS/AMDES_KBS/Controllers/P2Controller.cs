using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mommosoft.ExpertSystem;
using System.Windows.Controls;

namespace AMDES_KBS.Controllers
{
    class P2Controller
    {
        private List<string> Assertstrings;//string rules to be asserted for this decision tree
        private Mommosoft.ExpertSystem.Environment _theEnv = new Mommosoft.ExpertSystem.Environment();

        private string rule_path; //rule path in txt

        private string CLP_FILE =  @"engine\dementia.clp"; //clp file

        public string File_Name;

        private static List<string> attributes = new List<string>();//a attribute placeholder to make attribute distinct

        public P2Controller(string filename, string rule_path)
        {
            File_Name = filename;
            this.rule_path = rule_path;

            if (!rule_path.ToLower().Substring(rule_path.Length - 4, 4).Equals(".txt"))
                throw new InvalidOperationException("invalid rule file");

        }

        private List<rules> AllRules = new List<rules>();//all the rules in object form
        public class rules
        {
            public List<string> _lhs;
            public List<string> _compare;
            public List<string> _rhs;

            public string result;
            public Double agree;
            public double disagree;



            public rules()
            {
                _lhs = new List<string>();
                _compare = new List<string>();
                _rhs = new List<string>();
                result = "";
            }

            public void Add_Result(string input)
            {

                int start = input.IndexOf('(');

                result = input.Substring(0, start);
                string temp = input.Substring(start + 1, input.Length - (start + 1) - 1);
                //determine if got disagree anot

                int i = temp.IndexOf('/');
                if (i == -1)
                {
                    agree = double.Parse(temp);
                }
                else
                {
                    agree = double.Parse(temp.Substring(0, i));
                    disagree = double.Parse(temp.Substring(i + 1, temp.Length - (i + 1)));
                }

            }

            public void Add(string input)
            {
                int i = input.IndexOfAny(new[] { '>', '<', '=' });
                _lhs.Add(input.Substring(0, i).Trim());


                int intTemp = input.IndexOfAny(new[] { '>', '<', '=' }, i + 1); // check if got these symbol
                if (intTemp == (i + 1))
                {//2
                    _compare.Add(input.Substring(i, 2));
                    _rhs.Add(input.Substring(i + 2));
                }
                else
                {//1
                    if (input.Substring(i, 1) == "=")//if = will convert to ==
                        _compare.Add("==");
                    else
                        _compare.Add(input.Substring(i, 1));
                    _rhs.Add(input.Substring(i + 1));
                }


            }
        }
        private void reload(ListView vars)
        {

            List<string> attributes = new List<string>();


        }



        private List<Tuple<string, bool, List<string>>> Get_Attributes(List<rules> _rules, List<Tuple<string, bool, List<string>>> AttChoices)
        {


            foreach (rules Rule_Instance in _rules)
            {
                for (int i = 0; i < Rule_Instance._lhs.Count; i++)
                {
                    string Rule_LHS = Rule_Instance._lhs[i];
                    string Rule_RHS = Rule_Instance._rhs[i];
                    string Rule_Compare = Rule_Instance._compare[i];

                    if (!attributes.Contains(Rule_LHS))
                    {
                        attributes.Add(Rule_LHS);//add new attribute in
                        AttChoices.Add(new Tuple<string, bool, List<string>>(Rule_LHS, true, new List<string>()));//all are nominal ny default until find > or <
                    }


                    //not nominal
                    foreach (Tuple<string, bool, List<string>> c in AttChoices)
                    {
                        if (c.Item1.Equals(Rule_LHS))
                        {
                            //add new choice?
                            if (!c.Item3.Contains(Rule_RHS))
                            {
                                c.Item3.Add(Rule_RHS);
                            }

                            //chk nominal anot
                            if (c.Item2 == true && (Rule_Compare.IndexOf(">") != -1 || Rule_Compare.IndexOf("<") != -1))
                            {

                                Tuple<string, bool, List<string>> temp = new Tuple<string, bool, List<string>>(c.Item1, false, c.Item3);
                                AttChoices.Remove(c);
                                AttChoices.Add(temp);


                            }
                            break;
                        }
                    }

                    //
                }
            }

            //special if rule consist of only f or t then means true false add the opposite


            //preprocess the rules into clips form
            for (int j = 0; j < _rules.Count(); j++)
            {
                rules r = _rules[j];
                //(LeafSet (LeafID ) )
                Assertstrings.Add("(LeafSet (LeafID " + (j + 1).ToString() + ") )");
                for (int k = 0; k < r._lhs.Count(); k++)
                {
                    string lhs = r._lhs[k];
                    string compare = r._compare[k];
                    string rhs = r._rhs[k];

                    Assertstrings.Add("(clause (LeafID " + (j + 1).ToString() + ") (CompareType " + compare + ") (CompareValue " + rhs + ") (CompareAttributeName \"" + lhs + "\"))");
                }

                Assertstrings.Add("(Conclusion (LeafID " + (j + 1).ToString() + ") (Diagnosis " + r.result + ") (Agree " + r.agree + ") (DisAgree " + r.disagree + ") )");
            }


            return AttChoices;
        }
        private List<Tuple<string, string, string>> Get_Result(ListView vars)
        {

            for (int z = 0; z < vars.Items.Count; z++)
            {
                string fact = "(Attribute (Name \"" + ((Tuple<string, string>)vars.Items[z]).Item1 + "\") (Value " + ((Tuple<string, string>)vars.Items[z]).Item2 + "))";
                _theEnv.AssertString(fact);
            }


            _theEnv.Run();
            _theEnv.Eval("(focus DecisionTree)");


            String evalStr = "(find-all-facts ((?f Answer)) TRUE)";

            MultifieldValue mv = ((MultifieldValue)_theEnv.Eval(evalStr));

            List<string> all_diag = new List<string>();
            List<string> all_Agree = new List<string>();
            List<string> all_disAgree = new List<string>();

            List<Tuple<string, string, string>> all_results = new List<Tuple<string, string, string>>();

            foreach (FactAddressValue fv in mv)
            {
                string diag = fv.GetFactSlot("Diagnosis").ToString();
                string Agree = fv.GetFactSlot("Agree").ToString();
                string disAgree = fv.GetFactSlot("DisAgree").ToString();

                all_diag.Add(diag);
                all_Agree.Add(Agree);
                all_disAgree.Add(disAgree);
                Tuple<string, string, string> temp_results = new Tuple<string, string, string>(diag, Agree, disAgree);
                all_results.Add(temp_results);
                //MessageBox.Show(temp1 + ":" + temp2 + ":" + temp3);

            }

            return all_results;

        }

        public List<Tuple<string, bool, List<string>>> init(List<Tuple<string, bool, List<string>>> AttChoices)
        {
            AllRules = load();
            //get attributes
            return Get_Attributes(AllRules, AttChoices);
            //repopulate_rules(AllRules);
        }

        public List<Tuple<string, string, string>> Click(ListView vars)//when a button is clicked
        {
            repopulate_rules(AllRules);//assert rules into clips
            return Get_Result(vars);
        }
        //  public string clipsasserted = "";
        private void repopulate_rules(List<rules> _rules)
        {



            //time to convert to clips

            _theEnv = new Mommosoft.ExpertSystem.Environment();
            _theEnv.Clear();
            _theEnv.Load(CLP_FILE);//missing something
            _theEnv.Reset();

            _theEnv.Eval("(focus DecisionTree)");

            foreach (string ss in Assertstrings)
            {
                _theEnv.AssertString(ss);
                //    clipsasserted = clipsasserted + ss;
            }
            //_theEnv.Run();
        }

        private List<rules> load()
        {
            List<string> strings = new List<string>();


            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(rule_path);
            while ((line = file.ReadLine()) != null)
            {
                strings.Add(line);
                //Console.WriteLine(line);
                counter++;
            }

            file.Close();

            string[] clauseArray = new[] { ">=", "<=", "=", ">", "<" };

            Assertstrings = new List<string>();

            List<rules> _rules = new List<rules>();

            rules _rule = new rules();

            for (int i = 0; i < strings.Count(); i++)
            {
                string temp_str_remainder = strings[i].TrimEnd();
                if (clauseArray.Any(temp_str_remainder.Contains) == true)
                {


                    int intAnd = temp_str_remainder.IndexOf(" AND");
                    if (intAnd == (temp_str_remainder.Length - 4))
                    {
                        _rule.Add(temp_str_remainder.Substring(0, temp_str_remainder.Length - 4));



                    }
                    else if (temp_str_remainder.IndexOf(':') >= 0)
                    {
                        _rule.Add(temp_str_remainder.Substring(0, temp_str_remainder.IndexOf(':')));
                        _rule.Add_Result(temp_str_remainder.Substring(temp_str_remainder.IndexOf(':') + 1, temp_str_remainder.Length - (temp_str_remainder.IndexOf(':') + 1)));
                        _rules.Add(_rule);
                        _rule = new rules();
                    }


                }
            }

            return _rules;





        }
    }
}


