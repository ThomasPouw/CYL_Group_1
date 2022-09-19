using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ammo : MonoBehaviour
{    
    [SerializeField] List<AmmoSlot> ammoSlots;

    [System.Serializable]
    private class AmmoSlot
    {
        public AmmoType ammoType;
        public int ammoAmmount;
    }

    public int DecreaseAmmo(AmmoType ammoType)
    {
        return ammoSlots.Where(x => x.ammoType == ammoType).Single().ammoAmmount--;
    }

    public int GetAmmoCount(AmmoType ammoType)
    {
        return ammoSlots.Where(x => x.ammoType == ammoType).Single().ammoAmmount;
    }

    public int IncreaseAmmo(AmmoType ammoType, int ammount)
    {
        return ammoSlots.Where(x => x.ammoType == ammoType).Single().ammoAmmount += ammount;
    }
}
