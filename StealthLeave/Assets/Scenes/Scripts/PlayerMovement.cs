using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float playerMoveSpeed = 2f;

    [SerializeField]
    private Transform movePoint;

    [SerializeField]
    private LayerMask notMovementLayer;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, playerMoveSpeed * Time.deltaTime);
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
 
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {

            if (Mathf.Abs(horizontalInput) == 1f)
            {
                Vector3 nextStepPosition = movePoint.position + new Vector3(horizontalInput, 0f, 0f);
                if (!Physics2D.OverlapCircle(nextStepPosition, .2f, notMovementLayer))
                {
                    movePoint.position = nextStepPosition;
                }
            }
            else if (Mathf.Abs(verticalInput) == 1f)
            {
                Vector3 nextStepPosition = movePoint.position + new Vector3(0f, verticalInput, 0f);
                if (!Physics2D.OverlapCircle(nextStepPosition, .2f, notMovementLayer))
                {
                    movePoint.position = nextStepPosition;
                }
            }

        }

    }
}
