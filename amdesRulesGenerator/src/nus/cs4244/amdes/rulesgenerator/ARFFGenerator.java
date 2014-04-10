package nus.cs4244.amdes.rulesgenerator;

import java.util.*;
import java.io.*;
//Independence assumed, that race, gender, age is independent of the test taken
//i.e. Whether u are the same person or not, I dont care, I just want to know if a person with this set of attribute has taken a test or not

public class ARFFGenerator {

    public static void generateARFF(String key, List<PatientTest> noList, List<PatientTest> yesList) {
        try {

            //System.out.println(key);
            key = key.replace('/', '_').replace(':', '-');
            String path = Main.getJarContainingFolder(Main.class) + "\\" + key + ".arff";
            FileWriter write = new FileWriter(path, false);
            PrintWriter print_line = new PrintWriter(write);
            Integer size = noList.size() + yesList.size();

            print_line.println("% Num Instances: " + size.toString());
            print_line.println("% Num Attributes: 4");
            print_line.println("% Yes, No. |  classes");
            print_line.println("% age: continuous.");
            print_line.println("% Gender: MALE, FEMALE.");
            print_line.println("% EthnicGroup: CHINESE, MALAY, INDIAN, EURASIAN, OTHERS.");
            /////////////////////////////////////////////////////////////////////////////
            print_line.println("@relation " + key.replace(' ', '_'));
            print_line.println("@attribute 'age' numeric");
            print_line.println("@attribute 'Gender' {FEMALE, MALE}");
            print_line.println("@attribute 'EthnicGroup' {CHINESE, MALAY, INDIAN, EURASIAN, OTHERS}");
            print_line.println("@attribute 'class' {Yes, No}");
            print_line.println("@data");

            List<String> found = new ArrayList<String>();

            for (PatientTest p : yesList) {
                if (!found.contains(p.getUin())) {
                    found.add(p.getUin());
                    print_line.print(p.getAge());
                    print_line.print(",");
                    print_line.print(p.getGender().toString() + ",");
                    print_line.print(p.getRace().toString() + ",");
                    print_line.print("Yes");
                    print_line.println();
                    //Duplicate killing for yes only~
                } else {
                    break;
                }

            }
            //found.clear();
            for (PatientTest p : noList) {
               
                if (found.contains(p.getUin())) {
                     //if already have yes, then should not be considered
                } else {
                    print_line.print(p.getAge());
                    print_line.print(",");
                    print_line.print(p.getGender().toString() + ",");
                    print_line.print(p.getRace().toString() + ",");
                    print_line.print("No");
                    print_line.println();
                }

            }
            print_line.println("%");
            print_line.println("%");
            print_line.println("%No. of Patients Taken Test: " + found.size());
            print_line.close();

            WekaController.generateResults(key, path);

        } catch (Exception ex) {
            System.err.println(key);
            System.err.println(ex.getMessage());
            System.err.println(ex.getCause());
            // Logger.getLogger(ARFFGenerator.class.getName()).log(Level.SEVERE, null, ex);
        }

    }
//    //Buggy method
//    public static void generateARFF(List<PatientTest> pList) {
//        try {
//            String path = Main.getJarContainingFolder(Main.class) + "\\data.arff";
//            FileWriter write = new FileWriter(path, false);
//            PrintWriter print_line = new PrintWriter(write);
//            print_line.println("% Num Instances: " + pList.size());
//            print_line.println("% Num Attributes: 4");
//            print_line.println("% Yes, No. |  classes");
//            print_line.println("% age: continuous.");
//            print_line.println("% Gender: MALE, FEMALE.");
//            print_line.println("% EthnicGroup: CHINESE, MALAY, INDIAN, EURASIAN, OTHERS.");
//
//            print_line.print("% Test Done: CALCIUM, HOLTER(24HR AMBULATORY ECG), LIVER PANEL, ");
//            print_line.print("ALKALINE PHOSPHATASE, OCCULT BLOOD -  STOOL (IMMUNO METHOD), ");
//            print_line.print("THYROID PANEL, UFE WITH DIPSTIX 7, BMD HIP & LUMBAR SPINE, ");
//            print_line.print("ALBUMIN, CALCIUM PANEL, CYCLIC CITRULLINATED PEPTIDE AB, ");
//            print_line.print("ASPARTATE AMINOTRANSFERASE, LIPID PANEL 2 A, ALBUMIN:CREATININE RATIO, ");
//            print_line.print("RHEUMATOID FACTOR, US DOPPLER DVT -  LOWER LIMB, XR SHOULDER -  AP/LATERAL -  RIGHT, ");
//            print_line.print("LIPID MONITORING PANEL, GLYCATED HB, RENAL PANEL WITH GLUCOSE, XR CHEST -  AP/PA, ");
//            print_line.print("XR HIP -  AP/LATERAL -  LEFT,  THYROID PEROXIDASE AB, 25-HYDROXY VITAMIN D, ");
//            print_line.print("CARDIAC ENZYME PANEL, XR HAND -  AP/OBLIQUE -  BILATERAL, RENAL PANEL, TRANSTHORACIC ECHO - ROUTINE FULL, ");
//            print_line.print("US ABDOMEN,ANAEMIA PANEL 2,ANAEMIA PANEL 1, NUCLEAR AB, FOLATE, MR BRAIN -  DEMENTIA SCREENING, ");
//            print_line.print("XR SHOULDER -  AP/LATERAL -  LEFT, D-DIMER, SODIUM, GLUCOSE -  FASTING VENOUS, XR KUB, ");
//            print_line.print("CHOLESTEROL, FULL BLOOD COUNT, URIC ACID,  POTASSIUM, ALANINE AMINOTRANSFERASE, DSDNA AB, ");
//            print_line.print("XR PELVIS -  AP, ECG - 12 LEAD, VITAMIN B12, ERYTHROCYTE SEDIMENTATION RATE");
//            print_line.println();
//
//            print_line.println("@relation data");
//            print_line.println("@attribute 'age' integer");
//            print_line.println("@attribute 'Gender' { FEMALE, MALE}");
//            print_line.println("@attribute 'EthnicGroup' {CHINESE, MALAY, INDIAN, EURASIAN, OTHERS}");
//            print_line.print("@attribute 'Test' {");
//            print_line.print("CALCIUM,HOLTER(24HR AMBULATORY ECG),LIVER PANEL,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("ALKALINE PHOSPHATASE,OCCULT BLOOD -  STOOL (IMMUNO METHOD),".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("THYROID PANEL, UFE WITH DIPSTIX 7, BMD HIP & LUMBAR SPINE,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("ALBUMIN, CALCIUM PANEL, CYCLIC CITRULLINATED PEPTIDE AB,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("ASPARTATE AMINOTRANSFERASE, LIPID PANEL 2 A, ALBUMIN:CREATININE RATIO,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("RHEUMATOID FACTOR, US DOPPLER DVT -  LOWER LIMB, XR SHOULDER - AP/LATERAL -  RIGHT,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("LIPID MONITORING PANEL, GLYCATED HB, RENAL PANEL WITH GLUCOSE,XR CHEST -  AP/PA,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("XR HIP -  AP/LATERAL -  LEFT,THYROID PEROXIDASE AB,25-HYDROXY VITAMIN D,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("CARDIAC ENZYME PANEL, XR HAND -  AP/OBLIQUE -  BILATERAL,RENAL PANEL, TRANSTHORACIC ECHO - ROUTINE FULL,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("US ABDOMEN,ANAEMIA PANEL 2,ANAEMIA PANEL 1, NUCLEAR AB, FOLATE,MR BRAIN -  DEMENTIA SCREENING,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("XR SHOULDER -  AP/LATERAL -  LEFT, D-DIMER, SODIUM,GLUCOSE -  FASTING VENOUS,XR KUB, ".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("CHOLESTEROL,FULL BLOOD COUNT,URIC ACID,POTASSIUM,ALANINE AMINOTRANSFERASE, DSDNA AB,".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("XR PELVIS -  AP,ECG - 12 LEAD,VITAMIN B12,ERYTHROCYTE SEDIMENTATION RATE".replace(" ", "").replace('/', '_').replace(':', '-'));
//            print_line.print("}");
//            print_line.println();
//
//
//            print_line.println("@data");
//
//            for (PatientTest p : pList) {
//                print_line.print(p.getAge());
//                print_line.print(",");
//                print_line.print(p.getGender().toString() + ",");
//                print_line.print(p.getRace().toString() + ",");
//                print_line.print(p.getTestName().replace(" ", "").replace('/', '_').replace(':', '-'));
//                print_line.println();
//            }
//            print_line.println("%");
//            print_line.println("%");
//            print_line.println("%");
//            print_line.close();
//
//        } catch (Exception ex) {
//
//            System.err.println(ex.getMessage());
//            System.err.println(ex.getCause());
//            // Logger.getLogger(ARFFGenerator.class.getName()).log(Level.SEVERE, null, ex);
//        }
//
//    }
}
