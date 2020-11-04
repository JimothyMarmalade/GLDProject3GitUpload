using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWeapons : MonoBehaviour
{
    public Color TransparentWhite;
    public Color TransparentBlack;
    public Color Invisible;

    public Image PistolBackground;
    public Image PistolImage;
    public TMP_Text PistolAmmoText;

    public Image ShotgunBackground;
    public Image ShotgunImage;
    public TMP_Text ShotgunAmmoText;

    public Image SMGBackground;
    public Image SMGImage;
    public TMP_Text SMGAmmoText;

    public TMP_Text ObjectiveTracker;



    // Start is called before the first frame update
    void Start()
    {
        HideAll();
        ShowPistol();

        ObjectiveTracker.text = "Enemies Killed: ";
    }

    public void MakeAllTransparent()
    {
        MakeTransparent(PistolBackground);
        MakeTransparent(PistolImage);
        MakeTransparent(PistolAmmoText);
        MakeTransparent(ShotgunBackground);
        MakeTransparent(ShotgunImage);
        MakeTransparent(ShotgunAmmoText);
        MakeTransparent(SMGBackground);
        MakeTransparent(SMGImage);
        MakeTransparent(SMGAmmoText);
    }

    public void MakeTransparent(Image i)
    {
        //Debug.Log("Make Transparent");
        Color trans = i.color;

        trans.a = 70;

        i.color = TransparentWhite;

    }

    public void MakeTransparent(TMP_Text i)
    {
        Color trans = i.color;

        trans.a = 70;

        i.color = TransparentBlack;
    }

    public void MakeOpaque(Image i)
    {
        Color trans = i.color;

        trans.a = 255;

        i.color = Color.white;
    }

    public void MakeOpaque(TMP_Text i)
    {
        Color trans = i.color;

        trans.a = 255;

        i.color = Color.black;
    }

    public void Hide(Image i)
    {
        i.color = Invisible;
    }

    public void Hide(TMP_Text i)
    {
        i.color = Invisible;
    }

    public void Show (Image i)
    {
        i.color = Color.white;
    }

    public void Show (TMP_Text i)
    {
        i.color = Color.black;
    }

    public void HideAll()
    {
        Hide(PistolBackground);
        Hide(PistolImage);
        Hide(PistolAmmoText);
        Hide(ShotgunBackground);
        Hide(ShotgunImage);
        Hide(ShotgunAmmoText);
        Hide(SMGBackground);
        Hide(SMGImage);
        Hide(SMGAmmoText);
    }

    public void ShowPistol()
    {
        Show(PistolBackground);
        Show(PistolImage);
        Show(PistolAmmoText);
    }

    public void ShowShotgun()
    {
        Show(ShotgunBackground);
        Show(ShotgunImage);
        Show(ShotgunAmmoText);
    }

    public void ShowSMG()
    {
        Show(SMGBackground);
        Show(SMGImage);
        Show(SMGAmmoText);
    }

    public void HidePistol()
    {
        Hide(PistolBackground);
        Hide(PistolImage);
        Hide(PistolAmmoText);
    }

    public void HideShotgun()
    {
        Hide(ShotgunBackground);
        Hide(ShotgunImage);
        Hide(ShotgunAmmoText);
    }

    public void HideSMG()
    {
        Hide(SMGBackground);
        Hide(SMGImage);
        Hide(SMGAmmoText);
    }
    
    public void ShowPistolTransparent()
    {
        MakeTransparent(PistolBackground);
        MakeTransparent(PistolImage);
        MakeTransparent(PistolAmmoText);
    }

    public void ShowShotgunTransparent()
    {
        MakeTransparent(ShotgunBackground);
        MakeTransparent(ShotgunImage);
        MakeTransparent(ShotgunAmmoText);
    }

    public void ShowSMGTransparent()
    {
        MakeTransparent(SMGBackground);
        MakeTransparent(SMGImage);
        MakeTransparent(SMGAmmoText);
    }


    public void MakeAllTransparentExcept(int i)
    {
        //Debug.Log("MakeAllTransParentExcept");
        //All Except Pistol
        if (i == 0)
        {
            MakeOpaque(PistolBackground);
            MakeOpaque(PistolImage);
            MakeOpaque(PistolAmmoText);

            MakeTransparent(ShotgunBackground);
            MakeTransparent(ShotgunImage);
            MakeTransparent(ShotgunAmmoText);
            MakeTransparent(SMGBackground);
            MakeTransparent(SMGImage);
            MakeTransparent(SMGAmmoText);
        }
        //All Except Shotgun
        else if (i == 1)
        {
            MakeOpaque(ShotgunBackground);
            MakeOpaque(ShotgunImage);
            MakeOpaque(ShotgunAmmoText);

            MakeTransparent(PistolBackground);
            MakeTransparent(PistolImage);
            MakeTransparent(PistolAmmoText);
            MakeTransparent(SMGBackground);
            MakeTransparent(SMGImage);
            MakeTransparent(SMGAmmoText);
        }

        else if (i == 2)
        {
            MakeOpaque(SMGBackground);
            MakeOpaque(SMGImage);
            MakeOpaque(SMGAmmoText);

            MakeTransparent(PistolBackground);
            MakeTransparent(PistolImage);
            MakeTransparent(PistolAmmoText);
            MakeTransparent(ShotgunBackground);
            MakeTransparent(ShotgunImage);
            MakeTransparent(ShotgunAmmoText);
        }
    }

    public void UpdateWeaponAmmo(int pistolAmmo, int shotgunAmmo, int SMGAmmo)
    {
        PistolAmmoText.text = "Ammo: " + pistolAmmo.ToString();
        ShotgunAmmoText.text = "Ammo: " + shotgunAmmo.ToString();
        SMGAmmoText.text = "Ammo: " + SMGAmmo.ToString();
    }

    public void UpdateObjective(int i, bool isLevel1)
    {
        string update = "Enemies Killed: " + i.ToString();

        if (isLevel1)
        {
            ObjectiveTracker.text = update + "/25";
            if (i >= 25)
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Level2Available = true;
        }
        else
            ObjectiveTracker.text = update;

    }


}
