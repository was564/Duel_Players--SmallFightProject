namespace Character
{
    public interface ControlPlayerInterface
    {
        public void DecreaseHp(int damage);
        public void ResetHp();
        public void StopFrame();
        public void ResumeFrame();
        public void LookAtEnemy();
    }
}