using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    public GameObject imageBox;
    public Button continueButton;
    public string[] sentences;
    public string currSentence;
    private bool typing = false;
    private float jumpPower = 10f;
    private int numJumps = 1;
    private float duration = 2.0f;
    private float typingSpeed = 0.05f;
    public Transform[] bouncePositions;
    public static DialogueManager instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        sentences = new string[]{
            "Six years now my mother gone to earth. This dew, light as footsteps of the dead. She often walked out here, craned her neck, considered the fruit, hundreds of globes in their leathery hides, figuring on custard and pudding, meringue and hollandaise.",
            "But her plans didn't work out.",
            "The tree goes on unceasingly—lemons fall and fold into earth and begin again— me, I come here as a salve against heat, come to languish, to let the soft bursts—",
            "essence of citrus, summer's distillate— drift into my face and settle. Water and gold brew in the quiet deeps at the far end of the season. Leaves swallow the body of light and the breath of water brims over.",
            "My hands cup each other the way hers did."
        };
        
    }

    void Start()
    {
        continueButton.onClick.AddListener(DisplayNextSentence);

        Sequence bounceSequence = DOTween.Sequence();
        bounceSequence.Append(imageBox.transform.DOJump(bouncePositions[1].position, jumpPower, numJumps, duration));
        // bounceSequence.AppendInterval(0.25f);
        bounceSequence.Append(imageBox.transform.DOJump(bouncePositions[2].position, jumpPower, numJumps, duration));
        bounceSequence.Append(imageBox.transform.DOJump(bouncePositions[0].position, jumpPower, numJumps, duration));
        bounceSequence.SetLoops(-1, LoopType.Restart);
        dialogueBox.SetActive(false);
    }

    public void UpdateSentences(string[] newSentences)
    {
        sentences = newSentences;
    }

    public void DisplayNextSentence()
    {
        Records.freeze = true;
        dialogueBox.SetActive(true);
        Debug.Log("working?");
        if (typing)
        {
            dialogueText.text = currSentence;
            StopAllCoroutines();
            typing = false;
            return;
        }
        if (sentences.Length == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences[0];
        currSentence = sentence;
        sentences = sentences[1..];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        Records.freeze = false;
        dialogueBox.SetActive(false);
    }

    IEnumerator TypeSentence(string sentence)
    {
        typing = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            DisplayNextSentence();
        }
    }
}
