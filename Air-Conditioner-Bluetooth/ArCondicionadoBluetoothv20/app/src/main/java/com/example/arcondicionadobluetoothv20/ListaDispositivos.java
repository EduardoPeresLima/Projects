package com.example.arcondicionadobluetoothv20;

import android.app.ListActivity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;

import java.util.Set;

public class ListaDispositivos extends ListActivity {

    private BluetoothAdapter bluetoothAdapter = null;
    public static final String ENDERECO_MAC = "UM ENDERECO MAC1123";
    public static final String NOME_BT = "UM NOME BTBT 3211";
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        bluetoothAdapter = BluetoothAdapter.getDefaultAdapter();

        ArrayAdapter<String> arrayBluetooth = new ArrayAdapter<>(this,android.R.layout.simple_list_item_1);

        Set<BluetoothDevice> dispositivosPareados = bluetoothAdapter.getBondedDevices();
        if(dispositivosPareados.size()>0)
        {
            for(BluetoothDevice dispositivos : dispositivosPareados){
                String nomeBT = dispositivos.getName();
                String macBT = dispositivos.getAddress();
                arrayBluetooth.add(nomeBT + "\n" + macBT);
            }
        }

        setListAdapter(arrayBluetooth);

    }

    @Override
    protected void onListItemClick(ListView l, View v, int position, long id) {
        super.onListItemClick(l, v, position, id);
        String informacao = ((TextView) v).getText().toString();
        String nome = informacao.substring(0,informacao.length()-18);
        String endereco = informacao.substring(informacao.length()-17);
        Toast.makeText(getApplicationContext(),nome+" "+endereco,Toast.LENGTH_LONG).show();
        Intent retornaValores = new Intent();
        retornaValores.putExtra(NOME_BT,nome);
        retornaValores.putExtra(ENDERECO_MAC,endereco);
        setResult(RESULT_OK,retornaValores);
        finish();
    }
}