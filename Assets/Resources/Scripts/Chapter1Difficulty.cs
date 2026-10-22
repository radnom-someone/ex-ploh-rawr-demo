using UnityEngine;

public static class Chapter1Difficulty
{
    private const string PretestCorrect = "C1_PretestCorrect";
    private const string PretestTotal = "C1_PretestTotal";
    private const string EncounterCorrect = "C1_EncounterCorrect";
    private const string EncounterTotal = "C1_EncounterTotal";

    public static void SavePretest(int correct, int total)
    {
        PlayerPrefs.SetInt(PretestCorrect, correct);
        PlayerPrefs.SetInt(PretestTotal, total);

        // Start fresh random encounter scores for this chapter run.
        PlayerPrefs.SetInt(EncounterCorrect, 0);
        PlayerPrefs.SetInt(EncounterTotal, 0);

        PlayerPrefs.Save();
    }

    public static void SaveRandomEncounter(int correct, int total)
    {
        int previousCorrect = PlayerPrefs.GetInt(EncounterCorrect, 0);
        int previousTotal = PlayerPrefs.GetInt(EncounterTotal, 0);

        PlayerPrefs.SetInt(EncounterCorrect, previousCorrect + correct);
        PlayerPrefs.SetInt(EncounterTotal, previousTotal + total);

        PlayerPrefs.Save();
    }

    public static float GetPerformance()
    {
        int correct =
            PlayerPrefs.GetInt(PretestCorrect, 0) +
            PlayerPrefs.GetInt(EncounterCorrect, 0);

        int total =
            PlayerPrefs.GetInt(PretestTotal, 0) +
            PlayerPrefs.GetInt(EncounterTotal, 0);

        if (total <= 0)
        {
            return 0f;
        }

        return (float)correct / total * 100f;
    }

    public static int GetDifficulty()
    {
        float performance = GetPerformance();

        if (performance >= 80f)
        {
            return 2; // Hard
        }
        else if (performance >= 50f)
        {
            return 1; // Normal
        }
        else
        {
            return 0; // Easy
        }
    }


}