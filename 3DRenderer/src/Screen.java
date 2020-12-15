import javax.swing.JPanel;
import java.awt.*;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;

public class Screen extends JPanel implements KeyListener {
    //time in between position checks
    double SleepTime = (double) 1000/30; //div 30 for 30fps
    double LastRefresh = 0;

    //view position
    public static double[] ViewFrom = new double[]{10,10,10};
    public static double[] ViewTo = new double[]{1,0,0};

    //2D representations of 3D objects (Shadows)
    static int NumPolygons = 0; //number of polygons
    static PolygonObject[] DrawablePolygons = new PolygonObject[100]; //polygon objects

    //3D objects
    static DPolygon[] DPolygons = new DPolygon[100];
    int[] newOrder;

    // Create shapes
    // initialise key listener
    public Screen(){
        //cube
        DPolygons[0] = new DPolygon(new double[] {0,2,2,0}, new double[] {0,0,2,2}, new double[]{0,0,0,0}, Color.BLUE); //bottom
        DPolygons[1] = new DPolygon(new double[] {0,2,2,0}, new double[] {0,0,2,2}, new double[] {2,2,2,2}, Color.RED); //top
        DPolygons[2] = new DPolygon(new double[] {2,2,2,2}, new double[] {0,2,2,0}, new double[] {0,0,2,2}, Color.GREEN); //north
        DPolygons[3] = new DPolygon(new double[] {0,2,2,0}, new double[] {0,0,0,0}, new double[] {0,0,2,2}, Color.GRAY); //east
        DPolygons[4] = new DPolygon(new double[] {0,0,0,0}, new double[] {0,2,2,0}, new double[] {0,0,2,2}, Color.YELLOW); //south
        DPolygons[5] = new DPolygon(new double[] {0,2,2,0}, new double[] {2,2,2,2}, new double[] {0,0,2,2}, Color.ORANGE); //west

        addKeyListener(this);
        setFocusable(true);
    }

    //Draw object to screen
    public void paintComponent(Graphics g){

        g.clearRect(0,0,2000,1200);
        g.drawString(System.currentTimeMillis()+"", 20,20);

        for(int i=0; i<NumPolygons; i++)
            DPolygons[i].updatePolygon();

        setOrder();
        for(int i=0; i<NumPolygons; i++)
            DrawablePolygons[newOrder[i]].drawPolygon(g);

        Update();
    }


    //find layer order of the polygons that make up the 3D object
    void setOrder(){
        double[] k = new double[NumPolygons];
        newOrder = new int[NumPolygons];

        for(int i=0; i<NumPolygons; i++){
            k[i] = DrawablePolygons[i].getAvgDist();
            newOrder[i] = i;
        }

        double temp;
        int tempr;
        for(int a=0; a<k.length-1; a++) {
            for (int b = 0; b < k.length-1; b++) {
                if(k[b] < k[b+1]){
                    temp = k[b];
                    tempr = newOrder[b];
                    newOrder[b] = newOrder[b+1];
                    k[b] = k[b+1];

                    newOrder[b+1] = tempr;
                    k[b+1] = temp;
                }
            }
        }
    }


    //refresh the screen for given limit (30fps)
    void Update(){
        while(true){
            double millis = System.currentTimeMillis() - LastRefresh;
            if(millis > SleepTime){
                LastRefresh = System.currentTimeMillis();
                repaint();
                break;
            }
            else{
                try{
                    Thread.sleep((long)(SleepTime - millis));
                } catch(Exception e){System.out.println("Error");}
            }
        }
    }

    @Override
    public void keyTyped(KeyEvent e) {

    }

    @Override
    public void keyPressed(KeyEvent e) {
        if(e.getKeyCode() == KeyEvent.VK_LEFT) ViewFrom[0] --;
        if(e.getKeyCode() == KeyEvent.VK_RIGHT) ViewFrom[0] ++;
        if(e.getKeyCode() == KeyEvent.VK_UP) ViewFrom[1] --;
        if(e.getKeyCode() == KeyEvent.VK_DOWN) ViewFrom[1] ++;
    }

    @Override
    public void keyReleased(KeyEvent e) {

    }
}
