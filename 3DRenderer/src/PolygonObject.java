import java.awt.*;

public class PolygonObject{
    private final Polygon P;
    private final Color c;
    private double AvgDist = 0;

    public PolygonObject(double[] x, double[] y, Color c){

        P = new Polygon();
        for(int i=0; i<x.length; i++){
            P.addPoint((int)x[i], (int)y[i]);
        }
        this.c = c;
        Screen.NumPolygons++;
    }

    void drawPolygon (Graphics g){
        g.setColor(c);
        g.fillPolygon(P);
        g.setColor(Color.black);
        g.drawPolygon(P);
    }

    public double getAvgDist() {
        return AvgDist;
    }

    public void setAvgDist(double avgDist) {
        AvgDist = avgDist;
    }
}
