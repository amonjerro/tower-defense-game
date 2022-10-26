using System.Collections;

class VectorNode{

    public int sortable_value;
    private CoordinateVector data;

    public VectorNode parent = null;
    public VectorNode next = null;

    public VectorNode(int sortable_value, CoordinateVector data){
        this.sortable_value = sortable_value;
        this.data = data;
    }

    public CoordinateVector GetData(){
        return this.data;
    }


}

class PriorityQueue{

    private VectorNode root;

    public int count;

    public PriorityQueue(VectorNode root){
        this.root = root;
        this.count = 1;
    }

    public PriorityQueue(){
        this.count = 0;
    }


    public void Add(VectorNode node){
        this.count++;

        if (this.root == null){
            this.root = node;
            return;
        }

        VectorNode current = this.root;
        VectorNode previous = null; 

        if (node.sortable_value < current.sortable_value){
            node.next = current;
            current.parent = node;
            this.root = node;
            return;
        }
        
        while(current != null){
            if (node.sortable_value < current.sortable_value){
                current.parent.next = node;
                node.next = current;
                node.parent = current.parent;
                current.parent = node;
                return;
            }
            previous = current;
            current = current.next;
        }

        previous.next = node;
        node.parent = previous;
        return;
    }

    public CoordinateVector PopRoot(){
        this.count--;
        VectorNode returnable = this.root;

        this.root = this.root.next;

        return returnable.GetData();
    }

    public VectorNode Peek(){
        return this.root;
    }

    public bool Find(CoordinateVector node){
        VectorNode current = this.root;

        while (current != null){
            if (node == current.GetData()){
                return true;
            }
            current = current.next;
        }

        return false;
    }

    public void Clear(){
        this.count = 0;
        VectorNode current = this.root;
        VectorNode temp;
        this.root = null;

        while(current != null){
            temp = current.next;
            current.next = null;
            current.parent = null;
            current = temp;
        }
    }

}