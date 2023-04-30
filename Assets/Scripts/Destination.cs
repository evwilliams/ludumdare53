using System;

public class Destination : AreaOfInterest
{
    public DestinationChannel outputChannel;
    
    public void DropoffSucceeded(int rating)
    {
        _pendingDestroy = true;
        bubbleText.text = $"Rated {rating} {StarTextForRating(rating)}!";
        Destroy(gameObject, 2);
    }
    
    public void DropoffMismatch(int rating)
    {
        _pendingDestroy = true;
        bubbleText.text = $"That's not my baby! {rating} {StarTextForRating(rating)}!";
        Destroy(gameObject, 2);
    }
    
    public void DropoffMissed(int rating)
    {
        _pendingDestroy = true;
        bubbleText.text = $"I'm taking my stork business elsewhere! {rating} {StarTextForRating(rating)}!";
        Destroy(gameObject, 2);
    }

    private String StarTextForRating(int rating)
    {
        return rating == 1 ? "star" : "stars";
    }

    public override AOIChannel GetOutputChannel()
    {
        return outputChannel;
    }
}
