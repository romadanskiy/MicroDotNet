package com.example.scanner.presentation

import android.app.Activity
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.widget.ImageView
import androidx.core.app.ActivityCompat
import com.example.scanner.R
import com.example.scanner.domain.models.GarbageCategory
import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.presentation.viewmodels.*
import com.google.android.material.bottomnavigation.BottomNavigationView
import org.koin.androidx.viewmodel.ext.android.sharedViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel

class MainActivity : AppCompatActivity() {

    private val scannerViewModel by viewModel<ScannerViewModel>()
    private val scanningResultViewModel by viewModel<ScanningResultViewModel>()
    private val errorViewModel by viewModel<ErrorViewModel>()
    private val addGarbageViewModel by viewModel<AddGarbageViewModel>()
    private val GarbageCategoriesViewModel by viewModel<GarbageCategoriesViewModel>()
    private val receptionPointsViewModel by viewModel<ReceptionPointsViewModel>()
    private val addReceptionPointViewModel by viewModel<AddReceptionPointViewModel>()

    private val selectImageCode = 100000001

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)


        var currentFragmentId = -1;
        var bottomNav  = findViewById<BottomNavigationView>(R.id.bottom_navigation);

        bottomNav.setOnNavigationItemSelectedListener { item ->
            when (item.itemId) {
                R.id.nav_scanner -> {
                    if(currentFragmentId != R.id.nav_scanner){
                        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ScannerFragment()).commit();
                        currentFragmentId = R.id.nav_scanner;
                    }
                }
                R.id.nav_utilization -> {
                    if(currentFragmentId != R.id.nav_utilization){
                        receptionPointsViewModel.clear()
                        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ReceptionPointsFragment()).commit();
                        currentFragmentId = R.id.nav_utilization;
                    }
                }
                R.id.nav_profile -> {
                    if(currentFragmentId != R.id.nav_profile){
                        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ProfileFragment()).commit();
                        currentFragmentId = R.id.nav_profile;
                    }
                }
                R.id.nav_login -> {
                    if(currentFragmentId != R.id.nav_login){
                        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, LoginFragment()).commit();
                        currentFragmentId = R.id.nav_login;
                    }
                }
            }
            true;
        }

        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ScannerFragment()).commit();

    }

    fun onClick(v: View?) {
        if(v != null){
            when (v!!.id) {
                R.id.addGarbage -> {
                    var supportFragmentManager = supportFragmentManager;
                    supportFragmentManager?.beginTransaction()
                        ?.replace(R.id.fragment_container, AddGarbageFragment())?.commit();
                }

                R.id.goToAddGarbage -> {
                    var supportFragmentManager = supportFragmentManager;
                    supportFragmentManager?.beginTransaction()
                        ?.replace(R.id.fragment_container, AddGarbageFragment())?.commit();
                }
            }
        }

    }


}