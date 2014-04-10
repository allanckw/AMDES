package nus.cs4244.amdes.rulesgenerator;

import java.util.*;
import java.io.*;
import weka.classifiers.Evaluation;
import weka.classifiers.evaluation.NominalPrediction;
import weka.classifiers.rules.PART;
import weka.core.FastVector;
import weka.core.Instances;
import weka.classifiers.trees.J48;

public class WekaController {

    private static BufferedReader readDataFile(String filename) {
        BufferedReader inputReader = null;

        try {
            inputReader = new BufferedReader(new FileReader(filename));
        } catch (FileNotFoundException ex) {
            System.out.println("File not found: " + filename);
        }

        return inputReader;
    }

    private static Evaluation simpleClassify(PART model, Instances trainingSet, Instances testingSet) throws Exception {
        Evaluation validation = new Evaluation(trainingSet);


        model.buildClassifier(trainingSet);
        validation.evaluateModel(model, testingSet);

        return validation;

    }

    private static Evaluation simpleClassify(J48 model, Instances trainingSet, Instances testingSet) throws Exception {
        Evaluation validation = new Evaluation(trainingSet);


        model.buildClassifier(trainingSet);
        validation.evaluateModel(model, testingSet);

        return validation;

    }

    private static double calculateAccuracy(FastVector predictions) {
        double correct = 0;

        for (int i = 0; i < predictions.size(); i++) {
            NominalPrediction np = (NominalPrediction) predictions.elementAt(i);
            if (np.predicted() == np.actual()) {
                correct++;
            }


        }

        return 100 * correct / predictions.size();
    }

    private static double calculateAvgAreaUnderROC(List<Evaluation> evals, int cIndex) {
        double total = 0;
        for (int i = 0; i < evals.size(); i++) {
            Evaluation e = evals.get(i);
            double x = e.areaUnderPRC(cIndex);
            if (!Double.isNaN(x)) {
                total += x;
            }
            //System.out.println(e.areaUnderPRC(cIndex));
        }

        return 100 * total / evals.size();
    }

    private static Instances[][] crossValidationSplit(Instances data, int numberOfFolds) {
        Instances[][] split = new Instances[2][numberOfFolds];

        for (int i = 0; i < numberOfFolds; i++) {
            split[0][i] = data.trainCV(numberOfFolds, i);
            split[1][i] = data.testCV(numberOfFolds, i);
        }

        return split;
    }
    private static int validationSplits = 10;

    public static void generateResults(String key, String fName) throws Exception {
        BufferedReader datafile = readDataFile(fName);
        Instances data = new Instances(datafile);
        data.setClassIndex(data.numAttributes() - 1);

        // Choose a type of validation split
        Instances[][] split = crossValidationSplit(data, validationSplits);

        // Separate split into training and testing arrays
        Instances[] trainingSplits = split[0];
        Instances[] testingSplits = split[1];

        float bestConfFactor = getBestConfidenceFactor(trainingSplits, testingSplits);
        generatePartResult(key, bestConfFactor, trainingSplits, testingSplits);
        //generateJ48Result(bestConfFactor, trainingSplits, testingSplits);
    }
    //Class for generating a PART decision list. Uses separate-and-conquer. 
    //Builds a partial C4.5 decision tree in each iteration
    //and makes the "best" leaf into a rule.

    private static void generatePartResult(String key, float bestConfFactor,
            Instances[] trainingSplits, Instances[] testingSplits)
            throws Exception {
        System.out.println("Processing Test: " + key);
        PART partDecisionList = new PART();
        partDecisionList.setMinNumObj(1);
        partDecisionList.setReducedErrorPruning(false);
        partDecisionList.setUnpruned(true);
        partDecisionList.setUseMDLcorrection(true);
        partDecisionList.setConfidenceFactor(bestConfFactor);

        FastVector predictions = new FastVector();
        List<Evaluation> v = new ArrayList<Evaluation>();

        double attrCount = 0;
        int instances = 0;

        for (int j = 0; j < trainingSplits.length; j++) {
            Evaluation validation = simpleClassify(partDecisionList, trainingSplits[j], testingSplits[j]);

            instances += trainingSplits[j].size() + testingSplits[j].size();

            if (attrCount == 0) {
                attrCount = trainingSplits[j].numAttributes();
            }

            predictions.appendElements(validation.predictions());
            v.add(validation);
        }
        String dir = Main.getJarContainingFolder(Main.class) + "\\Rules\\";
        File f = new File(dir);
        if (!f.exists()) {
            if (f.mkdir()) {
                System.out.println("Directory is created!");
            } else {
                System.out.println("Failed to create directory!");
            }
        }
        String path = dir + key + ".txt";
        FileWriter write = new FileWriter(path, false);
        PrintWriter print_line = new PrintWriter(write);

        print_line.println(partDecisionList.toString());

        //some stats here.. can compute more
        print_line.println("====================================================");
        //accuracy most important tho
        print_line.println("Accuracy, Area under PRC - Yes, Area Under PRC - No");
        print_line.println(String.format("%.2f%%", calculateAccuracy(predictions)) + ",  " + String.format("%.2f%%", calculateAvgAreaUnderROC(v, 0)) + ",  " + String.format("%.2f%%", calculateAvgAreaUnderROC(v, 1)));

        print_line.println("Num of attr: " + attrCount);
        print_line.println("Num of instances:" + instances / validationSplits);

        print_line.close();
    }

    //J48 Tree Generated Does not Equal to the Tree PART Decision List Creates
    //Because J48 Tree builds everything first then determine the result
    //PART Decision List Creates the Best Leaf, Store the Leaf, then continue
    //Growing
    private static void generateJ48Result(float bestConfFactor,
            Instances[] trainingSplits, Instances[] testingSplits)
            throws Exception {

        J48 j48 = new J48();


        FastVector predictions = new FastVector();
        List<Evaluation> v = new ArrayList<Evaluation>();

        double attrCount = 0;
        int instances = 0;

        for (int j = 0; j < trainingSplits.length; j++) {
            Evaluation validation = simpleClassify(j48, trainingSplits[j], testingSplits[j]);

            instances += trainingSplits[j].size() + testingSplits[j].size();

            if (attrCount == 0) {
                attrCount = trainingSplits[j].numAttributes();
            }

            predictions.appendElements(validation.predictions());
            v.add(validation);
        }

        System.out.println(j48.toString());

    }

    private static float getBestConfidenceFactor(Instances[] trainingSplits, Instances[] testingSplits) throws Exception {
        double bestAccuracy = 0;
        float bestConfFactor = 0;
        // Collect every group of predictions for current model in a FastVector
        FastVector predictions = new FastVector();

        // Choose the PART classifier.
        PART partDecisionList = new PART();
        partDecisionList.setMinNumObj(1);
        partDecisionList.setReducedErrorPruning(false);
        partDecisionList.setUnpruned(false);
        partDecisionList.setUseMDLcorrection(true);

        partDecisionList.setConfidenceFactor(0.001f);

        for (int i = 0; i < 9; i++) {
            predictions.clear();
            partDecisionList.setConfidenceFactor(partDecisionList.getConfidenceFactor() + 0.05f);

            for (int j = 0; j < trainingSplits.length; j++) {
                Evaluation validation = simpleClassify(partDecisionList, trainingSplits[j], testingSplits[j]);
                predictions.appendElements(validation.predictions());
            }
            // Calculate overall accuracy of current classifier on all splits
            double accuracy = calculateAccuracy(predictions);
            if (accuracy >= bestAccuracy) {
                bestConfFactor = partDecisionList.getConfidenceFactor();
                bestAccuracy = accuracy;
            }
        }
        return bestConfFactor;

    }
//    public static void bleah(String[] args) throws Exception {
//
//        //TODO: Read Raw File
//
//        //TODO: Convert to ARFF file in some directory
//
//        //TODO: change readDataFile to that new ARFF File
//
//        BufferedReader datafile = readDataFile("C:\\Program Files\\Weka-3-7\\data\\sick.arff");
//
//        Instances data = new Instances(datafile);
//        data.setClassIndex(data.numAttributes() - 1);
//
//        int validationSplits = 10;
//
//        // Choose a type of validation split
//        Instances[][] split = crossValidationSplit(data, validationSplits);
//
//        // Separate split into training and testing arrays
//        Instances[] trainingSplits = split[0];
//        Instances[] testingSplits = split[1];
//
//        // Choose the PART classifier.
//        PART partDecisionList = new PART();
//        J48 j48 = new J48();
//
//        // Collect every group of predictions for current model in a FastVector
//        FastVector predictions = new FastVector();
//        List<Evaluation> v = new ArrayList<Evaluation>();
//
//        double bestAccuracy = 0;
//        float bestConfFactor = 0;
//
//        //find best confidence factor
//        partDecisionList.setConfidenceFactor(0.001f);
//
//        for (int i = 0; i < 9; i++) {
//            predictions.clear();
//            partDecisionList.setConfidenceFactor(partDecisionList.getConfidenceFactor() + 0.05f);
//
//            for (int j = 0; j < trainingSplits.length; j++) {
//                Evaluation validation = simpleClassify(partDecisionList, trainingSplits[j], testingSplits[j]);
//                predictions.appendElements(validation.predictions());
//            }
//            // Calculate overall accuracy of current classifier on all splits
//            double accuracy = calculateAccuracy(predictions);
//            if (accuracy >= bestAccuracy) {
//                bestConfFactor = partDecisionList.getConfidenceFactor();
//                bestAccuracy = accuracy;
//            }
//        }
//
//        predictions.clear();
//        double attrCount = 0;
//        int instances = 0;
//        //Final Model...
//        partDecisionList.setConfidenceFactor(bestConfFactor);
//        // For each training-testing split pair, train and test the classifier
//        for (int j = 0; j < trainingSplits.length; j++) {
//            Evaluation validation = simpleClassify(partDecisionList, trainingSplits[j], testingSplits[j]);
//
//            instances += trainingSplits[j].size() + testingSplits[j].size();
//
//            if (attrCount == 0) {
//                attrCount = trainingSplits[j].numAttributes();
//            }
//
//            predictions.appendElements(validation.predictions());
//            v.add(validation);
//        }
//
//        j48.setConfidenceFactor(bestConfFactor);
//
//        for (int j = 0; j < trainingSplits.length; j++) {
//            Evaluation validation = simpleClassify(j48, trainingSplits[j], testingSplits[j]);
//
//            instances += trainingSplits[j].size() + testingSplits[j].size();
//
//            if (attrCount == 0) {
//                attrCount = trainingSplits[j].numAttributes();
//            }
//
//            predictions.appendElements(validation.predictions());
//            v.add(validation);
//        }
//
//        //Rules here..
//        System.out.println(partDecisionList.toString());
//
//        //some stats here.. can compute more
//        System.out.println("====================================================");
//        //accuracy most important tho
//        System.out.println("Accuracy, Area under ROC - Sick, Area Under ROC - Negative");
//        System.out.println(String.format("%.2f%%", calculateAccuracy(predictions)) + ",  " + String.format("%.2f%%", calculateAvgAreaUnderROC(v, 0)) + ",  " + String.format("%.2f%%", calculateAvgAreaUnderROC(v, 1)));
//
//        System.out.println("Num of attr:" + attrCount);
//        System.out.println("Num of instances:" + instances / validationSplits);
//        //TODO: Output the rules to some other file
//
//        //TODO: Convert the rule files to a CLP file
//
//        //TODO: Run The CLP File, --> Test OK -->
//        //Create WPF / Java UI Application to create such an interface
//
//        //WPF FOr Clips:  CLIPSNET: http://clips.codeplex.com/
//
//        //JAVA For Clips: CLIPSJNI: http://sourceforge.net/projects/clipsrules/
//
//
//    }
}
