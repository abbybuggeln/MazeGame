using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Character Animator will instantiate the penguin, loop 
 * through the solution path, and move the penguin towards 
 * the next coordinate at a desired constant speed. 
 * 
 * It will also play the penguin animations.
 */
public class CharacterAnimator : MonoBehaviour
{
    private Animator characterAnimator;

    public void AnimateCharacter(List<Vector2> solutionPath, GameObject characterPrefab)
    {
        //Use a coroutine, animation will be continuous 
        StartCoroutine(Animate(solutionPath, characterPrefab));
    }

    private IEnumerator Animate(List<Vector2> solutionPath, GameObject characterPrefab)
    {
        //Instantiate at (0,0) as prompt states.
        GameObject character = Instantiate(characterPrefab, new Vector2(0, 0), Quaternion.identity);
        characterAnimator = character.GetComponent<Animator>();

        if (solutionPath == null || solutionPath.Count == 0)
        {
            Debug.Log("No path to animate");
            yield break;
        }

        characterAnimator.SetBool("IsWalking", true);
        character.transform.position = new Vector2(solutionPath[0].x, -solutionPath[0].y);

        foreach (var step in solutionPath)
        {
            //Here I add 0.2f to the y axis because the penguin sprite is off center and I don't quite have time to fix it 
            Vector2 newPosition = new Vector2(step.x, -step.y + 0.2f); 
            while (Vector2.Distance(character.transform.position, newPosition) > 0.1f)
            {
                character.transform.position = Vector2.MoveTowards(character.transform.position, newPosition, Time.deltaTime * 2);
                yield return null;
            }
        }

        characterAnimator.SetBool("IsWalking", false);
    }


}
