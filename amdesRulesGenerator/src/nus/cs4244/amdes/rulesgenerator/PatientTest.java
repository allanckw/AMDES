/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package nus.cs4244.amdes.rulesgenerator;

public class PatientTest implements Comparable<PatientTest> {

    public int compareTo(PatientTest o) {
        return this.testName.compareTo(o.testName);
    }

    public enum RACE {

        CHINESE,
        MALAY,
        INDIAN,
        EURASIAN,
        OTHERS
    }

    public enum SEX {

        MALE,
        FEMALE
    }
    private int age;
    private RACE race;
    private SEX gender;
    private String testName;
    private String uin;

    public int getAge() {
        return age;
    }

    public void setAge(int age) {
        this.age = age;
    }

    public RACE getRace() {
        return race;
    }

    public void setRace(RACE race) {
        this.race = race;
    }

    public SEX getGender() {
        return gender;
    }

    public void setGender(SEX gender) {
        this.gender = gender;
    }

    public String getTestName() {
        return testName;
    }

    public void setTestName(String testName) {
        this.testName = testName;
    }

    public String getUin() {
        return uin;
    }

    public void setUin(String uin) {
        this.uin = uin;
    }
}
