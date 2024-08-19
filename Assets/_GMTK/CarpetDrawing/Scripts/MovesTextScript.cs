using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovesTextScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private PatternGrid gridPattern;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movesText.text = "Moves done: " + gridPattern.oldGrid.Count;
        moneyText.text = "Money spent: " + gridPattern.oldGrid.Count + "$";
    }
}
