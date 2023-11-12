using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.InMenu.MenuOptions
{
    public class GameEndOption : MenuOptionInterface
    {
        public GameEndOption(GameObject optionObject) : base(optionObject, "Go To Menu") {}

        public override void OnSelect()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}