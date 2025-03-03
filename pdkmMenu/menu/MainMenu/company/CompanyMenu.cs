using Unity.Netcode;
using UnityEngine;


public class CompanyMenu : MonoBehaviour
{
    private guiBase companyMenuGUI;

    private void Start()
    {
        companyMenuGUI = gameObject.AddComponent<guiBase>();
        companyMenuGUI.MenuColor = companyMenuGUI.CustomBlue;
        companyMenuGUI.XPercentage = 0.07f;
        companyMenuGUI.YPercentage = 0.0f;
    }


    private static float companyBuyingRate = 0f;
    public void update()
    {
        companyMenuGUI.ButtonIndex = 0; // Reset index

        if (StartOfRound.Instance == null ) return;
        companyMenuGUI.AddButton("Attack Players", () =>
        {
            Object.FindObjectOfType<DepositItemsDesk>().AttackPlayersServerRpc();
        });
        companyMenuGUI.AddButton("Set Patience", () =>
        {
            Object.FindObjectOfType<DepositItemsDesk>().SetPatienceServerRpc(-3);
        });
        
        //Causes the items to breaks
        //companyMenuGUI.AddButton("Add all Items to desk", () =>
        //{
        //    DepositItemsDesk desk = FindObjectOfType<DepositItemsDesk>();
        //    if (desk != null)
        //    {
        //        foreach (GrabbableObject item in FindObjectsOfType<GrabbableObject>())
        //        {
        //            if (item.GetComponent<NetworkObject>() != null)
        //            {
        //                desk.AddObjectToDeskServerRpc(item.gameObject.GetComponent<NetworkObject>());
        //            }
        //        }
        //    }
        //});

        if (StartOfRound.Instance.IsServer)
        {
            DepositItemsDesk desk = Object.FindObjectOfType<DepositItemsDesk>();
            if (desk != null)
            {
                companyMenuGUI.AddButton("ToggleDoor", () => { desk.OpenShutDoorClientRpc(!Object.FindObjectOfType<DepositItemsDesk>().doorOpen); }, desk.doorOpen);
                companyMenuGUI.AddButton("MakeNoise", () => { desk.MakeWarningNoiseClientRpc(); });
            }

            if (companyBuyingRate == 0)
            {
                companyBuyingRate = StartOfRound.Instance.companyBuyingRate;
            }
            companyBuyingRate = companyMenuGUI.AddSlider(0.1f, 10, companyBuyingRate, companyBuyingRate.ToString(), StartOfRound.Instance == null);
            companyMenuGUI.AddButton("Set Buy Rate", () =>
            {
                StartOfRound.Instance.companyBuyingRate = companyBuyingRate;
                StartOfRound.Instance.SyncCompanyBuyingRateServerRpc();
            }, StartOfRound.Instance == null);
        }
    }
}

