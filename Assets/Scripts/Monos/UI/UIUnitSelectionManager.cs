using System;
using ECS.Systems;
using UnityEngine;

namespace UI {
    public class UIUnitSelectionManager : MonoBehaviour {
        [SerializeField] GameObject selectionAreaRect;

        void Start() {
            UnitSelectionSystem.Instance.OnSelectionStart += Handle_OnSelectAreaStart;
            UnitSelectionSystem.Instance.OnSelectionEnd += Handle_OnSelectAreaEnd;
            selectionAreaRect.SetActive(false);
        }

        void Handle_OnSelectAreaStart(object sender, EventArgs e) {
            selectionAreaRect.SetActive(true);
        }

        void Handle_OnSelectAreaEnd(object sender, EventArgs e) {
            selectionAreaRect.SetActive(false);
        }
    }
}