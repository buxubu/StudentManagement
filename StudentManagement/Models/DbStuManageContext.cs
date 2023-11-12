using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StudentManagement.Models
{
    public partial class DbStuManageContext : DbContext
    {
        public DbStuManageContext()
        {
        }

        public DbStuManageContext(DbContextOptions<DbStuManageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Fee> Fees { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentFee> StudentFees { get; set; } = null!;
        public virtual DbSet<StudyProgram> StudyPrograms { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<TestResult> TestResults { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=DbStuManage;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.IdAccount)
                    .HasName("PK__Account__DA18132C5199D200");

                entity.ToTable("Account");

                entity.Property(e => e.IdAccount).HasColumnName("idAccount");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createDate");

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.IdStudent).HasColumnName("idStudent");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("lastLogin");

                entity.Property(e => e.NameRole)
                    .HasMaxLength(20)
                    .HasColumnName("nameRole");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Salt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("salt");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__lastLog__32E0915F");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.IdStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__idStude__33D4B598");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.IdClass)
                    .HasName("PK__Class__17317A5AF024560F");

                entity.ToTable("Class");

                entity.Property(e => e.IdClass).HasColumnName("idClass");

                entity.Property(e => e.Descrip)
                    .HasMaxLength(50)
                    .HasColumnName("descrip");

                entity.Property(e => e.NameClass)
                    .HasMaxLength(20)
                    .HasColumnName("nameClass");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.IdCourse)
                    .HasName("PK__Course__8982E309FC98F43E");

                entity.ToTable("Course");

                entity.Property(e => e.IdCourse).HasColumnName("idCourse");

                entity.Property(e => e.CourseCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("courseCode");

                entity.Property(e => e.EndYear).HasColumnName("endYear");

                entity.Property(e => e.NameCourse)
                    .HasMaxLength(100)
                    .HasColumnName("nameCourse");

                entity.Property(e => e.StartYear).HasColumnName("startYear");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.IdDepart)
                    .HasName("PK__Departme__DEC204A6A083289D");

                entity.ToTable("Department");

                entity.Property(e => e.IdDepart).HasColumnName("idDepart");

                entity.Property(e => e.NameDepart)
                    .HasMaxLength(20)
                    .HasColumnName("nameDepart");
            });

            modelBuilder.Entity<Fee>(entity =>
            {
                entity.HasKey(e => e.IdFees)
                    .HasName("PK__Fee__6A74DE229E087F32");

                entity.ToTable("Fee");

                entity.Property(e => e.IdFees).HasColumnName("idFees");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.IdSub).HasColumnName("idSub");

                entity.HasOne(d => d.IdSubNavigation)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.IdSub)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Fee__amount__3D5E1FD2");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.IdPay)
                    .HasName("PK__Payment__3D783EEA86FC3B18");

                entity.ToTable("Payment");

                entity.Property(e => e.IdPay).HasColumnName("idPay");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.IdStuFee).HasColumnName("idStuFee");

                entity.HasOne(d => d.IdStuFeeNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.IdStuFee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__amount__440B1D61");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK__Role__E5045C54845CF9F9");

                entity.ToTable("Role");

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.Descrip)
                    .HasMaxLength(50)
                    .HasColumnName("descrip");

                entity.Property(e => e.NameRole)
                    .HasMaxLength(20)
                    .HasColumnName("nameRole");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.IdStudent)
                    .HasName("PK__Student__35B1F88A4F515CCA");

                entity.ToTable("Student");

                entity.Property(e => e.IdStudent).HasColumnName("idStudent");

                entity.Property(e => e.Birthday)
                    .HasColumnType("datetime")
                    .HasColumnName("birthday");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(20)
                    .HasColumnName("firstName");

                entity.Property(e => e.IdClass).HasColumnName("idClass");

                entity.Property(e => e.IdCourse).HasColumnName("idCourse");

                entity.Property(e => e.IdDepart).HasColumnName("idDepart");

                entity.Property(e => e.IdStuPro).HasColumnName("idStuPro");

                entity.Property(e => e.LastName)
                    .HasMaxLength(20)
                    .HasColumnName("lastName");

                entity.Property(e => e.NameClass)
                    .HasMaxLength(20)
                    .HasColumnName("nameClass");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.Sex)
                    .HasMaxLength(20)
                    .HasColumnName("sex");

                entity.Property(e => e.StudentCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("studentCode");

                entity.HasOne(d => d.IdClassNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.IdClass)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__idClass__300424B4");

                entity.HasOne(d => d.IdCourseNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.IdCourse)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__idCours__2E1BDC42");

                entity.HasOne(d => d.IdDepartNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.IdDepart)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__birthda__2D27B809");

                entity.HasOne(d => d.IdStuProNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.IdStuPro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__idStuPr__2F10007B");
            });

            modelBuilder.Entity<StudentFee>(entity =>
            {
                entity.HasKey(e => e.IdStuFee)
                    .HasName("PK__StudentF__3EFD96E5D5DD8BD1");

                entity.ToTable("StudentFee");

                entity.Property(e => e.IdStuFee).HasColumnName("idStuFee");

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreate");

                entity.Property(e => e.IdFees).HasColumnName("idFees");

                entity.Property(e => e.IdStudent).HasColumnName("idStudent");

                entity.Property(e => e.TotalFee).HasColumnName("totalFee");

                entity.HasOne(d => d.IdFeesNavigation)
                    .WithMany(p => p.StudentFees)
                    .HasForeignKey(d => d.IdFees)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentFe__idFee__412EB0B6");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.StudentFees)
                    .HasForeignKey(d => d.IdStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentFe__dateC__403A8C7D");
            });

            modelBuilder.Entity<StudyProgram>(entity =>
            {
                entity.HasKey(e => e.IdStuPro)
                    .HasName("PK__StudyPro__357C8D478EE964C8");

                entity.ToTable("StudyProgram");

                entity.Property(e => e.IdStuPro).HasColumnName("idStuPro");

                entity.Property(e => e.Level)
                    .HasMaxLength(50)
                    .HasColumnName("level");

                entity.Property(e => e.NameStuPro)
                    .HasMaxLength(50)
                    .HasColumnName("nameStuPro");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.IdSub)
                    .HasName("PK__Subject__024F281B15AE651A");

                entity.ToTable("Subject");

                entity.Property(e => e.IdSub).HasColumnName("idSub");

                entity.Property(e => e.DefaultMoney).HasColumnName("defaultMoney");

                entity.Property(e => e.IdStudent).HasColumnName("idStudent");

                entity.Property(e => e.PracticeCredits).HasColumnName("practiceCredits");

                entity.Property(e => e.Semester).HasColumnName("semester");

                entity.Property(e => e.SubjectCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("subjectCode");

                entity.Property(e => e.SubjectName)
                    .HasMaxLength(50)
                    .HasColumnName("subjectName");

                entity.Property(e => e.TheoryCredits).HasColumnName("theoryCredits");

                entity.Property(e => e.TotalCredits).HasColumnName("totalCredits");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.IdStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subject__practic__36B12243");
            });

            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.HasKey(e => e.IdTestRe)
                    .HasName("PK__TestResu__FB636D4FEFB65D8A");

                entity.ToTable("TestResult");

                entity.Property(e => e.IdTestRe).HasColumnName("idTestRe");

                entity.Property(e => e.IdStudent).HasColumnName("idStudent");

                entity.Property(e => e.IdSub).HasColumnName("idSub");

                entity.Property(e => e.Result).HasColumnName("result");

                entity.Property(e => e.ResultDes)
                    .HasMaxLength(50)
                    .HasColumnName("resultDes");

                entity.Property(e => e.TotalMarkFour).HasColumnName("totalMarkFour");

                entity.Property(e => e.TotalMarkString)
                    .HasMaxLength(10)
                    .HasColumnName("totalMarkString");

                entity.Property(e => e.TotalMarkTen).HasColumnName("totalMarkTen");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.IdStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__resul__398D8EEE");

                entity.HasOne(d => d.IdSubNavigation)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.IdSub)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__idSub__3A81B327");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
