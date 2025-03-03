// list-user.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User, UserService } from 'src/shared/services/user.service';

@Component({
  selector: 'app-list-user',
  templateUrl: './list-user.component.html',
  styleUrls: ['./list-user.component.css']
})
export class ListUserComponent implements OnInit {
  users: User[] = [];
  selectedUser: User | null = null;
  showModal = false;
  searchTerm = '';
  filteredUsers: User[] = [];

  // Form Fields
  name = '';
  email = '';
  NIF = '';
  password = '';

  constructor(
    private userService: UserService,
    private router: Router,

  ) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  openModal(user?: User) {
    if (user) {
      this.selectedUser = { ...user };
      this.name = user.Name;
      this.email = user.Email;
      this.NIF = user.NIF;
      // Reset password field for security
      this.password = '';
    } else {
      this.selectedUser = null;
      this.name = '';
      this.email = '';
      this.NIF = '';
      this.password = '';
    }
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.loadUsers();
  }

  saveUser() {
    // Create user data in the format expected by FastAPI's UserCreate model
    const userData = {
      name: this.name,
      email: this.email,
      nif: this.NIF,
      password: this.password
    };

    console.log('Saving user data:', userData);

    if (this.selectedUser) {
      // For updates, we need to handle the route with ID
      this.userService.updateUser({
        id: this.selectedUser.Id,
        ...userData
      }).subscribe({
        next: () => this.closeModal(),
        error: (error) => {
          console.error('Error updating user:', error);
          this.handleApiError(error);
        }
      });
    } else {
      this.userService.createUser(userData).subscribe({
        next: () => this.closeModal(),
        error: (error) => {
          console.error('Error creating user:', error);
          this.handleApiError(error);
        }
      });
    }
  }

  // Add this method to handle API errors
  handleApiError(error: any) {
    if (error.status === 422) {
      console.log('Validation error details:', error.error);
      console.log('Validation error: ' + JSON.stringify(error.error.detail || error.error));
    } else {
      console.log('An error occurred. Please try again.');
    }
  }

  deleteUser(id: string) {

  }

  searchUser() {
    if (!this.searchTerm.trim()) {
      this.filteredUsers = this.users;
      return;
    }

    const term = this.searchTerm.toLowerCase();
    this.filteredUsers = this.users.filter(user =>
      user.Name.toLowerCase().includes(term) ||
      user.Email.toLowerCase().includes(term) ||
      user.NIF.toLowerCase().includes(term)
    );
  }
  loadUsers() {
    this.userService.getUsers().subscribe(data => {
      this.users = data;
      this.filteredUsers = data;
    });
  }

  viewUser(user: any) {
    localStorage.setItem('selectedUser', JSON.stringify(user));
    console.log("User saved to localStorage:", user);

    // ✅ Navigate to Home Page with UserId as a query parameter
    this.router.navigate(['/home'], { queryParams: { userId: user.Id } });
  }

  logAction(userId: string, event: Event) {
    event.stopPropagation();

    this.userService.logUserAction(userId).subscribe({
      next: (response) => {
        console.log('User action logged successfully:', response);
      },
      error: (error) => {
        console.error('Error logging user action:', error);
      }
    });
  }
}

