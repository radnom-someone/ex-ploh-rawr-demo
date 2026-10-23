using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Flashcard
{
    public int chapter;
    public string term;
    public string definition;
}

public static class FlashcardInventory
{
    [Serializable]
    class SaveData { public List<Flashcard> cards = new List<Flashcard>(); }

    // New key, so old saves from the first version are ignored
    const string KEY = "FlashcardInventory_v2";
    static List<Flashcard> cards;

    public static event Action OnChanged;

    static void Load()
    {
        if (cards != null) return;
        string json = PlayerPrefs.GetString(KEY, "");
        cards = string.IsNullOrEmpty(json)
            ? new List<Flashcard>()
            : JsonUtility.FromJson<SaveData>(json).cards;
    }

    static void Save()
    {
        PlayerPrefs.SetString(KEY, JsonUtility.ToJson(new SaveData { cards = cards }));
        PlayerPrefs.Save();
        OnChanged?.Invoke();
    }

    public static List<Flashcard> GetCards(int chapter)
    {
        Load();
        return cards.FindAll(c => c.chapter == chapter);
    }

    public static void AddCard(int chapter, string term, string definition)
    {
        if (string.IsNullOrWhiteSpace(term)) return;
        Load();
        if (cards.Exists(c => c.chapter == chapter && c.term == term)) return;
        cards.Add(new Flashcard { chapter = chapter, term = term, definition = definition });
        Save();
    }

    public static void RemoveCard(int chapter, string term)
    {
        Load();
        if (cards.RemoveAll(c => c.chapter == chapter && c.term == term) > 0) Save();
    }

    public static void ClearAll()
    {
        cards = new List<Flashcard>();
        Save();
    }
}