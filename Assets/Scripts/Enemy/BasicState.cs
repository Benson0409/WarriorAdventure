public abstract class BasicState
{
    protected Enemy currentEnemy;
    //初始狀態
    public abstract void OnEnter(Enemy enemy);
    //邏輯狀態
    public abstract void LogicUpdate();
    //物理狀態
    public abstract void PhysicsUpdate();
    //離開狀態
    public abstract void OnExit();
}
