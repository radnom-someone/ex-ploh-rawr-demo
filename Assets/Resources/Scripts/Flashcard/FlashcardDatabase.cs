using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlashcardDatabase", menuName = "Scriptable Objects/Flashcard Database")]
public class FlashcardDatabase : ScriptableObject
{
    public List<Flashcard> cards = new List<Flashcard>();

    public List<Flashcard> GetCards(int chapter)
    {
        return cards.FindAll(c => c.chapter == chapter);
    }
}