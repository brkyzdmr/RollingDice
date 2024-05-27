using UnityEngine;

namespace Brkyzdmr.Services.DiceService
{
    [System.Serializable]
    public struct DiceData
    {
        public GameObject diceObject;
        public Rigidbody rb;
        public Dice diceLogic;
        public DiceContact diceContact;

        public DiceData(GameObject diceObject)
        {
            this.diceObject  = diceObject;
            this.rb          = diceObject.GetComponent<Rigidbody>();
            this.diceLogic   = diceObject.transform.GetComponentInChildren<Dice>();
            this.diceContact = diceObject.GetComponent<DiceContact>();
            this.rb.maxAngularVelocity = 1000;
        }
    }
}
