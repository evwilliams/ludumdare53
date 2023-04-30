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

    private void DisplayRandomResponse(Response[] responses, int rating)
    {
        var response = GetRandomResponse(responses);
        bubbleText.text = $"{response.displayText}\nRated {rating} {StarTextForRating(rating)}!";
        outputChannel.ResponseDisplayed(response);
    }
    
    public void DropoffSucceeded(int rating)
    {
        _pendingDestroy = true;
        DisplayRandomResponse(successResponses, rating);
        Destroy(gameObject, 2);
    }
    
    public void DropoffMismatch(int rating)
    {
        _pendingDestroy = true;
        DisplayRandomResponse(mismatchResponses, rating);
        Destroy(gameObject, 2);
    }
    
    public void DropoffMissed(int rating)
    {
        _pendingDestroy = true;
        DisplayRandomResponse(missResponses, rating);
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
