import java.awt.*;
import javax.swing.JFrame;

/*
Tutorials:
Java Tutorial - 3D from scratch
https://www.youtube.com/watch?v=gnT6YC5Nf70&list=PLsRmsZm0xMNogPyRn6gNWq4OM5j22FkAU
 */

public class DDDTutorial extends JFrame{
    private final static Dimension ScreenSize = Toolkit.getDefaultToolkit().getScreenSize();
    private static JFrame F = new DDDTutorial();
    private Screen ScreenObject = new Screen(); //get objects loaded in screen class

    public DDDTutorial(){

        add(ScreenObject); //draw objects to screen

        //setUndecorated(true); //remove button in top right
        setSize(ScreenSize); //set size of screen to fullscreen
        setVisible(true); //set window to visible
    }

    public static Dimension getScreenSize() {
        return ScreenSize;
    }

    public static void main(String[] args){

    }
}
