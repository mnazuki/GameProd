// placed on Energy at the hierarchy (add component) with 10 max count for energy

using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image energyBar;
    [SerializeField] private float energyMaxCount;
    public float _currentEnergyCount; //[NEW UPDATE] Changed to Public for references in other scripts.

    void Start()
    {
        _currentEnergyCount = energyMaxCount;
    }

    public void ReduceEnergyBar(float value){
        if (_currentEnergyCount >= 0){
            _currentEnergyCount -= value;
            UpdateEnergyBar();
        }
    }

    public bool CanUseEnergy(float value)
    {
        return _currentEnergyCount >= value;
    }

    public void UpdateEnergyBar(){
        energyBar.fillAmount = _currentEnergyCount / energyMaxCount;
    }
}
