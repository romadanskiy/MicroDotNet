package com.example.scanner.presentation.viewmodels

import androidx.collection.CircularArray
import androidx.fragment.app.FragmentManager
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.scanner.R
import com.example.scanner.presentation.*
import com.example.scanner.presentation.constants.FragmentIdConstants

class AppStateViewModel: ViewModel() {

    private val fragmentHistorySize = 20;
    private val fragmentsHistory: LiveData<CircularArray<Int>> = MutableLiveData(CircularArray(fragmentHistorySize))
    private val fragmentsIdMapping = mapOf(
        FragmentIdConstants.scannerFragmentId to ScannerFragment(),
        FragmentIdConstants.utilizationFragmentId to UtilizationFragment(),
        FragmentIdConstants.loginFragment to LoginFragment(),
        FragmentIdConstants.profileFragment to ProfileFragment(),
        FragmentIdConstants.errorFragment to ErrorFragment(),
        FragmentIdConstants.scanningResultFragment to ScanningResultFragment())


    fun changeFragment(supportFragmentManager: FragmentManager, newFragmentId: Int){
        val newFragment = fragmentsIdMapping[newFragmentId]!!;
        if(fragmentsHistory.value!!.last != R.id.nav_scanner){
            supportFragmentManager.beginTransaction().replace(R.id.fragment_container, newFragment).commit();
            addFragmentIdToHistory(newFragmentId);
        }
    }

    fun changeFragmentToPrev(supportFragmentManager: FragmentManager){
        if(removeLastFragmentIdFromHistory()){
            val prevFragment = fragmentsIdMapping[fragmentsHistory.value!!.last]!!;
            supportFragmentManager.beginTransaction().replace(R.id.fragment_container,prevFragment ).commit();
        }
    }

    private fun addFragmentIdToHistory(fragmentId: Int){
        if(fragmentsHistory.value!!.size()==fragmentHistorySize) {
            fragmentsHistory.value!!.removeFromStart(1);
        }
        fragmentsHistory.value!!.addLast(fragmentId);
    }

    private fun removeLastFragmentIdFromHistory(): Boolean{
        if(fragmentsHistory.value!!.isEmpty) {
            fragmentsHistory.value!!.popLast();
            return true
        }
        return false
    }
}