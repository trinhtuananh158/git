import com.sun.xml.internal.ws.transport.http.DeploymentDescriptorParser;

import java.io.*;
import java.lang.reflect.AccessibleObject;
import java.lang.reflect.Array;
import java.text.DateFormat;
import java.text.DateFormatSymbols;
import java.text.SimpleDateFormat;
import java.util.*;

class MyThread extends Thread{
    public void run(){
    }
}
public class Sample {
    public static void main(String[] agrs){
        int a=1;
        if(a==0)
        {
        }
        else
        {
            System.out.println("A!=0");
        }
    }
    static void showDir(int indent, File file)
            throws IOException {

    }
    public static boolean deleteDir(File dir) {
        if (dir.isDirectory()) {
            String[] children = dir.list();
            for (int i = 0; i < children.length; i++) {
                boolean success = deleteDir
                        (new File(dir, children[i]));
                if (!success) {
                    return false;
                }
            }
        }
        return dir.delete();
        //System.out.println("The directory is deleted.");
    }
}
