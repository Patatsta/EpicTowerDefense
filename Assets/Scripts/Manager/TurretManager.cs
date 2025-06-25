using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private GameObject _gatlingGun;
    [SerializeField] private GameObject _missileLauncher;
    [SerializeField] private GameObject _gatPlaceHolder, _misPlaceHolder, _currentPlaceHolder;
    [SerializeField] private GameObject _redMisPlaceHolder, _redGatPlaceHodler;
    public GameObject _upgradeButton, _dismantelButton;
    public bool IsEnabled { get; private set; } = false;
 

    private Collider _turretCollider;

    private void Start()
    {
        _upgradeButton.SetActive(false);
        _dismantelButton.SetActive(false);
        _turretCollider = GetComponent<Collider>();
        _gatlingGun.SetActive(false);
        _missileLauncher.SetActive(false);
        _gatPlaceHolder.SetActive(false);
        _misPlaceHolder.SetActive(false);
        _redMisPlaceHolder.SetActive(false);
        _redGatPlaceHodler.SetActive(false);
        _turretCollider.enabled = true;
    }

    public void Turret(int index)
    {
        if (IsEnabled) return;
        if (PlacementManager.Instance._turretStats[index].cost <= GameManager.Instance._warfunds)
        {
            GameManager.Instance.PlaceTurret(PlacementManager.Instance._turretStats[index].cost);
            PlacementManager.Instance.StopPlacing();
        }
        else
        {
            print("Not Enough Warfunds");
            return;
        }
        _gatPlaceHolder.SetActive(false);
        _misPlaceHolder.SetActive(false);
        _redMisPlaceHolder.SetActive(false);
        _redGatPlaceHodler.SetActive(false);
        //_turretCollider.enabled = false;

        if (index == 0)
        {
            _gatlingGun.SetActive(true);
        }
        else if (index == 1)
        {
            _missileLauncher.SetActive(true);
        }

        IsEnabled = true;
    }

    public void Dismantel(int index, bool isUpgrade)
    {
        int refund = isUpgrade ? PlacementManager.Instance._turretStats[index].doubleRefund : PlacementManager.Instance._turretStats[index].singleRefund;
        print(refund);
        GameManager.Instance.AddWarFunds(refund);
        IsEnabled = false;
        _gatlingGun.SetActive(false);
        _missileLauncher.SetActive(false);
        _gatPlaceHolder.SetActive(false);
        _misPlaceHolder.SetActive(false);
        _redMisPlaceHolder.SetActive(false);
        _redGatPlaceHodler.SetActive(false);
        _turretCollider.enabled = true;
    }

  

    public void TogglePlaceHolder(bool state, int index)
    {
        if (!IsEnabled)
        {
           
            if (!state)
            {
                _gatPlaceHolder.SetActive(false);
                _misPlaceHolder.SetActive(false);
                _redGatPlaceHodler.SetActive(false);
                _redMisPlaceHolder.SetActive(false);
                return;
            }

         
            switch (index)
            {
                case 0:
                    if (GameManager.Instance._warfunds >= PlacementManager.Instance._turretStats[index].cost)
                        _gatPlaceHolder.SetActive(true);
                    else
                        _redGatPlaceHodler.SetActive(true);
                    break;

                case 1:
                    if (GameManager.Instance._warfunds >= PlacementManager.Instance._turretStats[index].cost)
                        _misPlaceHolder.SetActive(true);
                    else
                        _redMisPlaceHolder.SetActive(true);
                    break;
            }
        }
    }

}



