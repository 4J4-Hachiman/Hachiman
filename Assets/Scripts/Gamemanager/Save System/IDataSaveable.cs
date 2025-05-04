/*
    Interface pour les objects qui auront des donnes a sauvegarder 
    dans les fichiers de sauvgarde;
        - Methode pour recuperer les donnes
        - Methode pour enregistrer les donnes

    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 03/05/2025;
*/
public interface IDataSaveable
{
    public void LoadData(GameData data);
    public void SaveData(ref GameData data);
}