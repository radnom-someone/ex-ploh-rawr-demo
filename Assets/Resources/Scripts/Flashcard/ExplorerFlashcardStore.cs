using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Local device storage. Does not require a scene object or a DontDestroyOnLoad object.
public static class ExplorerFlashcardStore
{
    private const string SaveKey = "EXplorer_Flashcards_v1";
    [Serializable] private class SaveData
    {
        public List<ExplorerFlashcardPanel.Card> cards = new List<ExplorerFlashcardPanel.Card>();
    }
    private static SaveData data;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        data = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) { RegisterLoadedQuizzes(); }
    private static void Load()
    {
        if (data != null) return;
        data = new SaveData();
        string json = PlayerPrefs.GetString(SaveKey, "");
        if (string.IsNullOrEmpty(json)) return;
        try
        {
            SaveData saved = JsonUtility.FromJson<SaveData>(json);
            if (saved != null && saved.cards != null) data = saved;
        }
        catch (Exception error) { Debug.LogError("Could not read flashcards: " + error.Message); }
    }
    private static void Save()
    {
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }
    private static bool Add(int chapter, QA qa, bool collected)
    {
        if (qa == null || string.IsNullOrWhiteSpace(qa.QUESTION) || string.IsNullOrWhiteSpace(qa.ANSWER)) return false;
        string term = qa.ANSWER;
        string definition = qa.QUESTION;
        if (qa is Trivia)
        {
            bool correct;
            if (!bool.TryParse(qa.ANSWER.Trim(), out correct))
            {
                Debug.LogWarning("Flashcard skipped: true/false answer must be true or false. " + qa.QUESTION);
                return false;
            }
            term = qa.QUESTION;
            if (correct) definition = "This is true";
            else definition = "This is false";
        }
        foreach (ExplorerFlashcardPanel.Card card in data.cards)
        {
            if (card == null) continue;
            if (card.chapter != chapter || card.term != term || card.definition != definition) continue;
            if (collected && !card.collected) { card.collected = true; return true; }
            return false;
        }
        data.cards.Add(new ExplorerFlashcardPanel.Card {
            chapter = chapter, term = term, definition = definition, collected = collected
        });
        return true;
    }
    public static void RegisterLoadedQuizzes()
    {
        Load(); bool changed = false;
        OBJ_CMD[] quizzes = UnityEngine.Object.FindObjectsByType<OBJ_CMD>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (OBJ_CMD quiz in quizzes)
        {
            if (quiz.QAs == null) continue;
            foreach (QA qa in quiz.QAs)
            {
                if (Add(Mathf.Max(1, quiz.flashcardChapter), qa, false)) changed = true;
            }
        }
        if (changed) Save();
    }
    public static void RegisterQuestions(int chapter, IEnumerable<QA> questions)
    {
        Load(); if (questions == null) return;
        bool changed = false;
        foreach (QA qa in questions) { if (Add(Mathf.Max(1, chapter), qa, false)) changed = true; }
        if (changed) Save();
    }
    public static void CollectFailures(int chapter, IEnumerable<QA> questions)
    {
        Load(); if (questions == null) return;
        bool changed = false;
        foreach (QA qa in questions) { if (Add(Mathf.Max(1, chapter), qa, true)) changed = true; }
        if (changed) Save();
    }
    public static List<ExplorerFlashcardPanel.Card> GetCards()
    {
        Load();
        return new List<ExplorerFlashcardPanel.Card>(data.cards);
    }
}
