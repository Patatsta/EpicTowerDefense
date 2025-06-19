using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private GameObject _gatlingGun;
    [SerializeField] private GameObject _missileLauncher;
    [SerializeField] private GameObject _gatPlaceHolder, _misPlaceHolder, _currentPlaceHolder;
    [SerializeField] private GameObject _redMisPlaceHolder, _redGatPlaceHodler;
   
    public bool IsEnabled { get; private set; } = false;
    [SerializeField] private List<TurretStats> TurretStats = new List<TurretStats>();

    private void Start()
    {
        _gatlingGun.SetActive(false);
        _missileLauncher.SetActive(false);
        _gatPlaceHolder.SetActive(false);
        _misPlaceHolder.SetActive(false);
        _redMisPlaceHolder.SetActive(false);
        _redGatPlaceHodler.SetActive(false);
    }

    public void Turret(int index)
    {
        if (IsEnabled) return;
        if (TurretStats[index].cost <= GameManager.Instance._warfunds)
        {
            GameManager.Instance.PlaceTurret(TurretStats[index].cost);
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
                    if (GameManager.Instance._warfunds >= TurretStats[index].cost)
                        _gatPlaceHolder.SetActive(true);
                    else
                        _redGatPlaceHodler.SetActive(true);
                    break;

                case 1:
                    if (GameManager.Instance._warfunds >= TurretStats[index].cost)
                        _misPlaceHolder.SetActive(true);
                    else
                        _redMisPlaceHolder.SetActive(true);
                    break;
            }
        }
    }

}

[System.Serializable]
public class TurretStats
{
    public int index;
    public int cost;
}

