import java.awt.*;

public class DPolygon {
    double[] x,y,z;
    Color c;
    int poly = 0;

    public DPolygon(double[] x, double[] y, double[] z, Color c){
        this.x = x;
        this.y = y;
        this.z = z;
        this.c = c;
        createPolygon();
    }

    public void createPolygon(){
        poly = Screen.NumPolygons;
        Screen.DrawablePolygons[poly] = new PolygonObject(new double[]{},new double[]{},c);
        updatePolygon();
    }

    public void updatePolygon() {
        int SCALE = 50;

        double[] center = Calculator.Calculate(Screen.ViewFrom, Screen.ViewTo, Screen.ViewTo[0], Screen.ViewTo[1], Screen.ViewTo[2]);
        double dx = -SCALE*center[0];
        double dy = -SCALE*center[1];

        double[] newX = new double[x.length];
        double[] newY = new double[x.length];

        for (int i = 0; i < x.length; i++) {
            double[] newPos = Calculator.Calculate(Screen.ViewFrom, Screen.ViewTo, x[i], y[i], z[i]);
            newX[i] = dx + DDDTutorial.getScreenSize().getWidth()/2 + SCALE* newPos[0];
            newY[i] = dy + DDDTutorial.getScreenSize().getHeight()/2 + SCALE* newPos[1];
        }

        Screen.DrawablePolygons[poly] = new PolygonObject(newX, newY, c);
        Screen.DrawablePolygons[poly].setAvgDist(getDist());
        Screen.NumPolygons --;
    }

    private double getDist(){
        double total = 0;
        for(int i=0; i<x.length; i++)
            total += getDistToP(i);

        return total/x.length;
    }

    private double getDistToP(int i){
        double dx = Screen.ViewFrom[0] -x[i];
        double dy = Screen.ViewFrom[1] -y[i];
        double dz = Screen.ViewFrom[2] -z[i];

        return Math.sqrt(dx*dx + dy*dy + dz*dz);
    }
}
