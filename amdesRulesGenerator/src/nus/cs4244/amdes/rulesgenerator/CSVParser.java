package nus.cs4244.amdes.rulesgenerator;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.*;

public class CSVParser {

    public static boolean header = true;

    public static void run(String csvFile) {

        BufferedReader br = null;
        String line = "";
        String csvSplitBy = ",";
        int count = 0;
        List<PatientTest> pList = new ArrayList<PatientTest>();

        try {

            br = new BufferedReader(new FileReader(csvFile));

            while ((line = br.readLine()) != null) {
                if (header && count == 0) {
                    //ignore header
                    //AGE, GENDER, RACE, TEST
                } else {
                    // use comma as separator
                    String[] record = line.split(csvSplitBy);
                    PatientTest pt = new PatientTest();
                    pt.setAge(Integer.parseInt(record[0]));
                    pt.setGender(PatientTest.SEX.valueOf(record[1].toUpperCase()));
                    pt.setRace(PatientTest.RACE.valueOf(record[2].toUpperCase()));
                    pt.setTestName(record[3].toUpperCase().trim());
                    pt.setUin(record[4].toUpperCase().trim());
                    pList.add(pt);

                    //System.out.println("Country [code= " + country[4] + " , name=" + country[5] + "]");
                }
                count++;
            }
            generateARFF(pList);

        } catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            if (br != null) {
                try {
                    br.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
        //ARFFGenerator.generateARFF(pList);
        System.out.println("Done");
    }

    private static void generateARFF(List<PatientTest> pList) {
        Collections.sort(pList);
        HashMap<String, List<PatientTest>> cache = new HashMap<String, List<PatientTest>>();

        for (int i = 0; i < pList.size(); i++) {
            PatientTest p = pList.get(i);
            //System.out.println(p .getTestName());
            if (!cache.containsKey(p .getTestName()))
            {
                List<PatientTest> l = new ArrayList<PatientTest>();
                l.add(p);
                cache.put(pList.get(i).getTestName(), l);
            }else {
                cache.get(p.getTestName()).add(p);
            }
            
        }

        for (String key : cache.keySet()) {
            List<PatientTest> yesList = cache.get(key);
            //System.out.println("Key: " + key + ", size: " + yesList.size());
            List<PatientTest> notList = cloneList(pList);
         
            //System.out.println(pList.size());
            for (PatientTest pt : yesList)
            {
                if(notList.contains(pt))
                    notList.remove(pt);
            }
            //System.out.println(notList.size());
            //System.out.println("--------------------------------------------------------");
            ARFFGenerator.generateARFF(key, notList, yesList);
        }
    }

    public static List<PatientTest> cloneList(List<PatientTest> src)
    {
        List<PatientTest> pList = new ArrayList<PatientTest>();
        for(PatientTest pt : src)
        {
            pList.add(pt);
        }
        return pList;
    }
}

