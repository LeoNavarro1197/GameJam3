using UnityEngine;

public class Menu : MonoBehaviour
{
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DownMenu()
    {
        animator.SetBool("down", true);
        animator.SetBool("up", false);
    }

    public void UpMenu()
    {
        animator.SetBool("down", false);
        animator.SetBool("up", true);
    }
}
