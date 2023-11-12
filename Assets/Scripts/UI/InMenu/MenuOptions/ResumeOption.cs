using UnityEngine;

namespace UI.InMenu.MenuOptions
{
    public class ResumeOption : MenuOptionInterface
    {
        private MenuInGame _menuManager;

        public ResumeOption(GameObject optionObject) : base(optionObject, "Resume")
        {
            _menuManager = GameObject.FindObjectOfType<MenuInGame>();
        }

        public override void OnSelect()
        {
            _menuManager.ControlMenu();
        }
    }
}