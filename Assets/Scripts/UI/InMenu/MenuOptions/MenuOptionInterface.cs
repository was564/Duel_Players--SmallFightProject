using System;
using TMPro;
using UnityEngine;

namespace UI.InMenu.MenuOptions
{
    public abstract class MenuOptionInterface
    {
        private TextMeshProUGUI _text;
        public RectTransform Transform { get; private set; }

        public MenuOptionInterface(GameObject optionObject, string optionName)
        {
            _text = optionObject.GetComponent<TextMeshProUGUI>();
            _text.text = optionName;
            Transform = optionObject.GetComponent<RectTransform>();
        }

        public abstract void OnSelect();
    }
}