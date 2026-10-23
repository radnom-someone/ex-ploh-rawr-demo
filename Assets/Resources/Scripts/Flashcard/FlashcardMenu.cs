using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlashcardMenu : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] FlashcardDatabase generalDatabase;
    [SerializeField] int totalChapters = 4;

    [Header("Card display (inside Card > Flashcard)")]
    [SerializeField] TMP_Text sideLabel;     // the "Text" under Flashcard ("Term" / "Definition")
    [SerializeField] TMP_Text cardText;      // TermDefinition Text
    [SerializeField] TMP_Text counterText;   // Index

    [Header("Card buttons")]
    [SerializeField] Button prevCardButton;  // Card Prev
    [SerializeField] Button nextCardButton;  // Card Next
    [SerializeField] Button flipButton;      // optional: a Button on the Card object

    [Header("Chapter selector")]
    [SerializeField] TMP_Text chapterLabel;  // Chapter Text
    [SerializeField] Button prevChapterButton; // Button Prev
    [SerializeField] Button nextChapterButton; // Button Next

    [Header("Type tabs")]
    [SerializeField] Button collectedTabButton; // Button Collected
    [SerializeField] Button generalTabButton;   // Button General
    [SerializeField] TMP_Text tabLabel;         // Current Tab Text

    int chapter = 1;
    bool showCollected = true;
    int index = 0;
    bool showingTerm = true;

    void Awake()
    {
        prevCardButton.onClick.AddListener(() => MoveCard(-1));
        nextCardButton.onClick.AddListener(() => MoveCard(1));
        prevChapterButton.onClick.AddListener(() => MoveChapter(-1));
        nextChapterButton.onClick.AddListener(() => MoveChapter(1));
        collectedTabButton.onClick.AddListener(() => SetTab(true));
        generalTabButton.onClick.AddListener(() => SetTab(false));

        if (flipButton != null) flipButton.onClick.AddListener(Flip);
        else Debug.LogWarning("FlashcardMenu: no Flip Button assigned, so definitions can't be shown.");
    }

    void OnEnable()
    {
        index = 0;
        showingTerm = true;
        Refresh();
    }

    // When the real database exists, change only this method.
    List<Flashcard> GetGeneralCards(int ch)
    {
        return generalDatabase != null ? generalDatabase.GetCards(ch) : new List<Flashcard>();
    }

    List<Flashcard> CurrentCards()
    {
        return showCollected ? FlashcardInventory.GetCards(chapter) : GetGeneralCards(chapter);
    }

    void SetTab(bool collected)
    {
        showCollected = collected;
        index = 0;
        showingTerm = true;
        Refresh();
    }

    void MoveChapter(int dir)
    {
        chapter = ((chapter - 1 + dir + totalChapters) % totalChapters) + 1;
        index = 0;
        showingTerm = true;
        Refresh();
    }

    void MoveCard(int dir)
    {
        int count = CurrentCards().Count;
        if (count == 0) return;
        index = (index + dir + count) % count;
        showingTerm = true;
        Refresh();
    }

    void Flip()
    {
        if (CurrentCards().Count == 0) return;
        showingTerm = !showingTerm;
        Refresh();
    }

    void Refresh()
    {
        List<Flashcard> cards = CurrentCards();
        bool hasCards = cards.Count > 0;

        chapterLabel.text = $"Chapter {chapter}";
        tabLabel.text = showCollected ? "Collected" : "General";
        collectedTabButton.interactable = !showCollected;
        generalTabButton.interactable = showCollected;

        prevCardButton.interactable = hasCards;
        nextCardButton.interactable = hasCards;
        if (flipButton != null) flipButton.interactable = hasCards;

        if (!hasCards)
        {
            sideLabel.text = "";
            cardText.text = "No flashcards yet";
            counterText.text = "0 / 0";
            return;
        }

        index = Mathf.Clamp(index, 0, cards.Count - 1);
        Flashcard card = cards[index];
        sideLabel.text = showingTerm ? "Term" : "Definition";
        cardText.text = showingTerm ? card.term : card.definition;
        counterText.text = $"{index + 1} / {cards.Count}";
    }
}