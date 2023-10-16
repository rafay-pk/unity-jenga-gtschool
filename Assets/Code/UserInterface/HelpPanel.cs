using DG.Tweening;

namespace Code.UserInterface
{
    public class HelpPanel : SlidingPanel
    {
        protected override void SetPanelState(bool state)
        {
            parent.DOAnchorPosY(state ? panel.sizeDelta.y : 0f, state ? slideInDuration : slideOutDuration)
                .SetEase(Ease.InOutSine);
        }
    }
}
