using UnityEngine;

public class Destination : AreaOfInterest
{
    public DestinationChannel outputChannel;

    public Response[] successResponses;
    public Response[] mismatchResponses;
    public Response[] missResponses;

    private Response GetRandomResponse(Response[] responses)
    {
        return responses[Random.Range(0, responses.Length)];
    }

    private void DisplayRandomResponse(Response[] responses)
    {
        var response = GetRandomResponse(responses);
        bubbleText.text = response.displayText;
        outputChannel.ResponseDisplayed(response);
        // bubbleText.text = $"Rated {rating} {StarTextForRating(rating)}!";
    }
    
    public void DropoffSucceeded(int rating)
    {
        _pendingDestroy = true;
        DisplayRandomResponse(successResponses);
        Destroy(gameObject, 2);
    }
    
    public void DropoffMismatch(int rating)
    {
        _pendingDestroy = true;
        DisplayRandomResponse(mismatchResponses);
        // bubbleText.text = $"That's not my baby! {rating} {StarTextForRating(rating)}!";
        Destroy(gameObject, 2);
    }
    
    public void DropoffMissed(int rating)
    {
        _pendingDestroy = true;
        DisplayRandomResponse(missResponses);
        // bubbleText.text = $"I'm taking my stork business elsewhere! {rating} {StarTextForRating(rating)}!";
        Destroy(gameObject, 2);
    }

    private string StarTextForRating(int rating)
    {
        return rating == 1 ? "star" : "stars";
    }

    public override AOIChannel GetOutputChannel()
    {
        return outputChannel;
    }
}
