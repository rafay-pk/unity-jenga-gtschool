using DG.Tweening;
namespace Code.UserInterface
{
    public class AttributionPanel : SlidingPanel
    {
        protected override void SetPanelState(bool state)
        {
            parent.DOAnchorPosX(state ? -panel.sizeDelta.x : 0f, state ? slideInDuration : slideOutDuration)
                .SetEase(Ease.InOutSine);
        }
    }
}
