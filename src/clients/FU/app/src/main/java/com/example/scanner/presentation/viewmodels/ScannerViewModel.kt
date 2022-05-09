package com.example.scanner.presentation.viewmodels

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.scanner.domain.models.Barcode

class ScannerViewModel: ViewModel() {
    private val mutableBarcode: MutableLiveData<Barcode> = MutableLiveData()
    val barcode : LiveData<Barcode> = mutableBarcode

    fun saveBarcode(barcodeString: String){
        mutableBarcode.postValue(Barcode(barcodeString))
    }

}