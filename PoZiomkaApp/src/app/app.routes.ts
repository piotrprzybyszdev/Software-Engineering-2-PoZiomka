import { CanMatchFn, RedirectCommand, Router, Routes } from "@angular/router";
import { routes as authRoutes } from "./auth/auth.routes";
import { NotFoundComponent } from "./not-found/not-found.component";
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { inject } from "@angular/core";
import { StudentService } from "./student/student.service";
import { AdminService } from "./admin/admin.service";

const canAccessStudent: CanMatchFn = (route, segments) => {
  const router = inject(Router);
  const studentService = inject(StudentService);

  if (studentService.loggedInStudent()) {
    return true;
  }
  //return true; // to do: fix auth
  return new RedirectCommand(router.parseUrl('/unauthorized/student'));
}

const canAccessAdmin: CanMatchFn = (route, segments) => {
  const router = inject(Router);
  const adminService = inject(AdminService);

  if (adminService.loggedInAdmin()) {
    return true;
  }

  return true; // to do: fix auth
  return new RedirectCommand(router.parseUrl('/unauthorized/admin'));
}

export const routes: Routes = [
  ...authRoutes,
  {
    path: 'student',
    loadComponent: () => import('./student/student.component').then(mod => mod.StudentComponent),
    title: 'Student',
    loadChildren: () => import('./student/student.routes').then(mod => mod.routes),
    canMatch: [canAccessStudent]
  },
  {
    path: 'admin',
    loadComponent: () => import('./admin/admin.component').then(mod => mod.AdminComponent),
    title: 'Admin',
    loadChildren: () => import('./admin/admin.routes').then(mod => mod.routes),
    canMatch: [canAccessAdmin]
  },
  {
    path: 'confirm-email/:token', 
    component: ConfirmEmailComponent, 
    title: 'Potwierdzenie E-maila'
  },
  {
    path: '**',
    title: 'Poziomka - Nie znaleziono',
    component: NotFoundComponent
  }
]