package com.example.scanner.presentation

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.example.scanner.R
import com.example.scanner.presentation.login.LoginFragment
import com.example.scanner.presentation.reg.RegFragment
import com.example.scanner.presentation.viewmodels.ScannerViewModel
import com.google.android.material.bottomnavigation.BottomNavigationView
import org.koin.androidx.viewmodel.ext.android.viewModel

class MainActivity : AppCompatActivity() {

    private val scannerViewModel by viewModel<ScannerViewModel>()
    private var currentFragmentId = -1

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)


        var bottomNav = findViewById<BottomNavigationView>(R.id.bottom_navigation);

        bottomNav.setOnNavigationItemSelectedListener { item ->
            when (item.itemId) {
                R.id.nav_scanner -> {
                    if (currentFragmentId != R.id.nav_scanner) {
                        supportFragmentManager.beginTransaction()
                            .replace(R.id.fragment_container, ScannerFragment()).commit();
                        currentFragmentId = R.id.nav_scanner;
                    }
                }
                R.id.nav_utilization -> {
                    if (currentFragmentId != R.id.nav_utilization) {
                        supportFragmentManager.beginTransaction()
                            .replace(R.id.fragment_container, UtilizationFragment()).commit();
                        currentFragmentId = R.id.nav_utilization;
                    }
                }
                R.id.nav_profile -> {
                    if (currentFragmentId != R.id.nav_profile) {
                        toProfile()
                    }
                }
                R.id.nav_login -> {
                    if (currentFragmentId != R.id.nav_login) {
                        supportFragmentManager.beginTransaction()
                            .replace(R.id.fragment_container, LoginFragment()).commit();
                        currentFragmentId = R.id.nav_login;
                    }
                }
            }
            true;
        }

        supportFragmentManager.beginTransaction()
            .replace(R.id.fragment_container, ScannerFragment()).commit();

    }

    fun toReq() {
        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, RegFragment())
            .commit();
    }

    fun toLogin() {
        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, LoginFragment())
            .commit();
    }

    fun toProfile() {
        supportFragmentManager.beginTransaction()
            .replace(R.id.fragment_container, ProfileFragment()).commit()
        currentFragmentId = R.id.nav_profile;

    }

}