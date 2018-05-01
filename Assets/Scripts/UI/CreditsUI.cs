using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour {

	// Closes the credits UI;
    public void CloseUI()
    {
        this.gameObject.SetActive(false);
    }
}
