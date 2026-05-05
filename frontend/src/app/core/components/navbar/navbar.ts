import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, AsyncPipe],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class Navbar {
  auth = inject(AuthService);

  getInitiales(user: any): string {
  if (!user) return '';
  const p = user.prenom?.charAt(0) ?? '';
  const n = user.nom?.charAt(0) ?? '';
  return (p + n).toUpperCase();
}
  
}
