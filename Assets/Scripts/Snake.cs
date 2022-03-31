using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    public int xSize, ySize;
    public GameObject block;
    GameObject head;
    public Material HeadColor, TailColor;
    List<GameObject> tail;
    Vector2 dir;
    GameObject food;
    bool isAlive;
    // Start is called before the first frame update
    void Start()
    {
        dir = Vector2.right;
        createGrid();
        createPlayer();
        spawnFood();
        block.SetActive(false);
        isAlive = true;
    }
    public Text points;
    float passedTime, timeBetweenMovements;
    bool top;
    bool down;
    bool left;
    bool right;
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) && down == true)
        {
            dir = Vector2.down;
            down = true; top = false; right = true; left = true;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && top == true)
        {
            dir = Vector2.up;
            down = false; top = true; right = true; left = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && right == true)
        {
            dir = Vector2.right;
            down = true; top = true; right = true; left = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && left == true)
        {
            dir = Vector2.left;
            down = true; top = true; right = false; left = true;
        }

        passedTime += Time.deltaTime;
        if (timeBetweenMovements < passedTime && isAlive)
        {
            passedTime = 0;

            Vector3 newPosition = head.GetComponent<Transform>().position + new Vector3(dir.x, dir.y, 0);

            if (newPosition.x >= xSize / 2 || newPosition.x <= -xSize / 2 || newPosition.y >= ySize / 2 || newPosition.y >= -ySize / 2)
            {

            }
            foreach (var item in tail)
            {
                if (item.transform.position == newPosition)
                {
                    gameOver();
                }
            }
            if (newPosition.x == food.transform.position.x && newPosition.y == food.transform.position.y)
            {
                GameObject newTile = Instantiate(block);
                newTile.SetActive(true);
                newTile.transform.position = food.transform.position;
                DestroyImmediate(food);
                head.GetComponent<MeshRenderer>().material = TailColor;
                tail.Add(head);
                head = newTile;
                head.GetComponent<MeshRenderer>().material = HeadColor;
                spawnFood();
                points.text = "Points: " + tail.Count;
            }
            else
            {
                if (tail.Count == 0)
                {
                    head.transform.position = newPosition;
                }
                else
                {
                    head.GetComponent<MeshRenderer>().material = TailColor;
                    tail.Add(head);
                    head = tail[0];
                    head.GetComponent<MeshRenderer>().material = HeadColor;
                    tail.RemoveAt(0);
                    head.transform.position = newPosition;
                }
            }
        }
    }
        private void createGrid()
        {
            for (int x = 0; x <= xSize; x++)
            {
                GameObject borderBottom = Instantiate(block) as GameObject;
                borderBottom.GetComponent<Transform>().position = new Vector3(x - xSize / 2, -ySize / 2, 0);

                GameObject borderTop = Instantiate(block) as GameObject;
                borderTop.GetComponent<Transform>().position = new Vector3(x - xSize / 2, ySize - ySize / 2, 0);
            }
            for (int y = 0; y <= xSize; y++)
            {
                GameObject borderRight = Instantiate(block) as GameObject;
                borderRight.GetComponent<Transform>().position = new Vector3(-xSize / 2, y - (ySize / 2), 0);

                GameObject borderLeft = Instantiate(block) as GameObject;
                borderLeft.GetComponent<Transform>().position = new Vector3(xSize - (xSize / 2), y - (ySize / 2), 0);
            }
        }
        private Vector2 getRandomPos()
        {
            return new Vector2(Random.Range(-xSize / 2 + 1, xSize / 2), Random.Range(-ySize / 2 + 1, ySize / 2));
        }
        private bool containedInSnake(Vector2 spawnPos)
        {
            bool isInHead = spawnPos.x == head.transform.position.x && spawnPos.y == head.transform.position.y;
            bool isInTail = false;
            foreach (var item in tail)
            {
                if (item.transform.position.x == spawnPos.x && item.transform.position.y == spawnPos.y)
                {
                    isInTail = true;
                }
            }
            return isInHead || isInTail;
        }
        
        private void spawnFood()
        {
            Vector2 spawnPos = getRandomPos();
            while (containedInSnake(spawnPos))
            {
                spawnPos = getRandomPos();
            }
            food = Instantiate(block);
            food.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0);
            food.SetActive(true);

    }
    private void createPlayer()
        {
            head = Instantiate(block) as GameObject;
            head.GetComponent<MeshRenderer>().material = HeadColor;
            tail = new List<GameObject>();
        }
    public GameObject gameOverUI;
    public void restart()
    {
        SceneManager.LoadScene(0);
    }
    private void gameOver()
    {
        isAlive = false;
        gameOverUI.SetActive(true);
    }
    
}
