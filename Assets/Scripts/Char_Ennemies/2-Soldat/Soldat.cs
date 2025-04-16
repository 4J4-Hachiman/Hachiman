/*
    Class principale des soldats
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 05/04/2025;
*/

public class Soldat : EnnemiMain
{
    private void Update()
    {
        SetAnimVParams();
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }
}
