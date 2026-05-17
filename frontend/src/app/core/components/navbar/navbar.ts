import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { AuthService, User } from '../../services/auth';
import { SearchBar } from '../../../shared/search-bar/search-bar';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, AsyncPipe, SearchBar],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class Navbar {
  auth = inject(AuthService);
  getInitiales(user: User | null): string {
    if (!user) return '';
    const p = user.prenom?.charAt(0) ?? '';
    const n = user.nom?.charAt(0) ?? '';
    return (p + n).toUpperCase();
  }
}