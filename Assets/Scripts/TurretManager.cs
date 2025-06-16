using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private GameObject _gatlingGun;
    [SerializeField] private GameObject _missileLauncher;
    [SerializeField] private GameObject _gatPlaceHolder, _misPlaceHolder, _currentPlaceHolder;

    public bool IsEnabled { get; private set; } = false;

    private void Start()
    {
        _gatlingGun.SetActive(false);
        _missileLauncher.SetActive(false);
        _gatPlaceHolder.SetActive(false);
        _misPlaceHolder.SetActive(false);
    }

    public void Turret(int index)
    {
        if (IsEnabled) return;
        _gatPlaceHolder.SetActive(false);
        _misPlaceHolder.SetActive(false);

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
            switch (index)
            {
                case 0:
                    _gatPlaceHolder.SetActive(state);
                    _currentPlaceHolder = _gatPlaceHolder;
                    break;
                case 1:
                    _misPlaceHolder.SetActive(state);
                    _currentPlaceHolder = _misPlaceHolder;
                    break;
                default:
                    break;
            }
        }
            
    }

    //public bool IsPlaceHolder()
    //{
    //    return _currentPlaceHolder.activeSelf;
    //}
}
