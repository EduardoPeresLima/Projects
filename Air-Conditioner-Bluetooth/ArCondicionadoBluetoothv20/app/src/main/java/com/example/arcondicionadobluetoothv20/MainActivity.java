package com.example.arcondicionadobluetoothv20;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.UUID;

public class MainActivity extends AppCompatActivity {
    ConnectedThread connectedThread = null;
    BluetoothAdapter bluetoothAdapter = null;
    BluetoothDevice bluetoothDevice = null;
    BluetoothSocket bluetoothSocket = null;
    private UUID mUUID = UUID.fromString("00001101-0000-1000-8000-00805f9b34fb");

    Switch btAr = null;
    Button btConectar = null;
    TextView txtBlue = null;
    TextView txtArCondicionado = null;

    boolean conexao = false;
    private Handler handler;
    private StringBuilder dadosBluetooth = new StringBuilder();


    private static String MACBT = null;
    private static String NOMEBT = null;
    private static final String RETORNA_ESTADO_AR = "i";
    private static final int SOLICITA_ATIVAR_BLUETOOTH = 1;
    private static final int SOLICITA_CONEXAO = 2;

    private static final int MESSAGE_READ = 10;

    @SuppressLint("HandlerLeak")
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        btConectar = findViewById(R.id.btConectar);
        btAr = findViewById(R.id.btArCondicionado);
        txtBlue = findViewById(R.id.txtBluetooth);
        txtArCondicionado = findViewById(R.id.txtArCondicionado);


        ligarBluetooth();

        btConectar.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v) {
                if(conexao)
                {
                    desconectarBluetooth();
                }
                else{
                    Intent abreLista = new Intent(getApplicationContext(), ListaDispositivos.class);
                    startActivityForResult(abreLista, SOLICITA_CONEXAO);
                }
            }
        });

        handler = new Handler(){
            @Override
            public void handleMessage(@NonNull Message msg) {
                if(msg.what == MESSAGE_READ)
                {
                    String recebidos = (String)msg.obj;

                    dadosBluetooth.append(recebidos);
                    int fim = dadosBluetooth.indexOf("}");
                    if(fim>0) {
                        String dadosCompletos = dadosBluetooth.substring(0,fim);
                        int tam = dadosCompletos.length();

                        if(dadosCompletos.charAt(0) == '{'){
                            String dadosFinais = dadosBluetooth.substring(1,tam);
                            Log.d("Recebidos",dadosFinais);
                            if(dadosFinais.contains("D")) {
                                btAr.setChecked(true);
                                btAr.setText("Ligado");
                            }
                            else if(dadosFinais.contains("L")) {
                                btAr.setChecked(false);
                                btAr.setText("Desligado");
                            }
                        }
                        dadosBluetooth.delete(0,dadosBluetooth.length());

                    }
                }
            }
        };
        btAr.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if(btAr.isChecked()){

                    btAr.setText("Ligado");
                    connectedThread.enviar("0");
                }
                else{
                    btAr.setText("Desligado");
                    connectedThread.enviar("1");
                }
            }
        });
    }

    private void ligarBluetooth() {
        bluetoothAdapter = BluetoothAdapter.getDefaultAdapter();

        if(bluetoothAdapter == null){
            Toast.makeText(getApplicationContext(),"Seu dispositivo não possui bluetooth",Toast.LENGTH_LONG).show();
        }
        else if(!bluetoothAdapter.isEnabled()){
            Intent intent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            startActivityForResult(intent,SOLICITA_ATIVAR_BLUETOOTH);
        }
    }


    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        switch (requestCode) {
            case SOLICITA_ATIVAR_BLUETOOTH:
                if (resultCode == Activity.RESULT_OK) {
                    int a;
                    Toast.makeText(this,"Bluetooth Ativado", Toast.LENGTH_LONG).show();
                } else {
                    Toast.makeText(this, "Bluetooth não foi ativado, App será encerrado", Toast.LENGTH_LONG).show();
                    finish();
                }
                break;
            case SOLICITA_CONEXAO:
                if (resultCode == Activity.RESULT_OK) {
                    MACBT = data.getExtras().getString(ListaDispositivos.ENDERECO_MAC);
                    NOMEBT = data.getExtras().getString(ListaDispositivos.NOME_BT);
                    conectarBluetooth();
                } else {
                    Toast.makeText(getApplicationContext(), "Falha ao obter o MAC", Toast.LENGTH_LONG).show();
                }
        }
    }


    private void conectarBluetooth() {
        try {
            bluetoothDevice = bluetoothAdapter.getRemoteDevice(MACBT);
            bluetoothSocket = bluetoothDevice.createRfcommSocketToServiceRecord(mUUID);
            bluetoothSocket.connect();

            btAr.setVisibility(Switch.VISIBLE);
            txtArCondicionado.setVisibility(TextView.VISIBLE);
            btConectar.setText("Desconectar Bluetooth");
            conexao = true;
            txtBlue.setText("Conectado a "+NOMEBT);

            connectedThread = new ConnectedThread(bluetoothSocket);
            connectedThread.start();
            connectedThread.enviar(RETORNA_ESTADO_AR);
        }
        catch (Exception erro){
            Toast.makeText(getApplicationContext(),"Ocorreu um erro : "+ erro,Toast.LENGTH_LONG).show();
        }
    }
    private void desconectarBluetooth() {
        try {
            bluetoothSocket.close();
            Toast.makeText(getApplicationContext(), "O Bluetooth foi desconectado", Toast.LENGTH_LONG).show();

            btAr.setVisibility(Switch.INVISIBLE);
            txtArCondicionado.setVisibility(TextView.INVISIBLE);
            btConectar.setText("Conectar Bluetooth");
            conexao = false;
            txtBlue.setText("Não Conectado");
            connectedThread.interrupt();
        } catch (IOException erro) {
            Toast.makeText(getApplicationContext(), "Ocorreu um erro: " + erro, Toast.LENGTH_LONG).show();
        }
    }

    private class ConnectedThread extends Thread {
        private final BluetoothSocket mmSocket;
        private final InputStream mmInStream;
        private final OutputStream mmOutStream;
        private byte[] mmBuffer;

        public ConnectedThread(BluetoothSocket socket) {
            mmSocket = socket;
            InputStream tmpIn = null;
            OutputStream tmpOut = null;

            // Get the input and output streams; using temp objects because
            // member streams are final.
            try {
                tmpIn = socket.getInputStream();
            } catch (IOException e) {
                Toast.makeText(getApplicationContext(), "Ocorreu um erro ao criar variavel de recebimento de dados", Toast.LENGTH_LONG).show();
            }
            try {
                tmpOut = socket.getOutputStream();
            } catch (IOException e) {
                Toast.makeText(getApplicationContext(), "Ocorreu um erro ao criar variavel que recebe dados", Toast.LENGTH_LONG).show();
            }

            mmInStream = tmpIn;
            mmOutStream = tmpOut;
        }

        public void run() {
            byte[] buffer = new byte[1024];
            int bytes;
            while(true){
                try{
                    bytes = mmInStream.read(buffer);
                    String dadosBT = new String(buffer, 0, bytes);
                    handler.obtainMessage(MESSAGE_READ,bytes,-1,dadosBT).sendToTarget();
                }
                catch (Exception e){
                    break;
                }
            }
        }

        public void enviar(String dado) {
            byte[] bytes = dado.getBytes();
            try {
                mmOutStream.write(bytes);

            } catch (IOException e) {
                Toast.makeText(getApplicationContext(),"Ocorreu um erro ao enviar dados", Toast.LENGTH_LONG).show();
            }
        }

    }
}
